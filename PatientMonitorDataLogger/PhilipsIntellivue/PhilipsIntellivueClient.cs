using System.IO.Ports;
using System.Text;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class PhilipsIntellivueClient : IDisposable, IAsyncDisposable
{
    private readonly PhilipsIntellivueClientSettings settings;
    private ISerialPort? serialPort;
    private SerialPortCommunicator? serialPortCommunicator;
    private readonly CommandMessageCreator messageCreator = new();
    private readonly object connectLock = new();
    private readonly Timer alertPollTimer;
    private readonly Timer numericsPollTimer;

    public PhilipsIntellivueClient(
        PhilipsIntellivueClientSettings settings)
    {
        this.settings = settings;
        alertPollTimer = new Timer(
            SendAlertPollRequest,
            null,
            Timeout.Infinite,
            Timeout.Infinite);
        numericsPollTimer = new Timer(
            SendNumericsPollRequest,
            null,
            Timeout.Infinite,
            Timeout.Infinite);
    }

    public bool IsConnected { get; private set; }
    public bool IsListening => serialPortCommunicator?.IsListening ?? false;
    public event EventHandler<ICommandMessage>? NewMessage;
    public event EventHandler<MonitorConnectionChangeEventType>? ConnectionStatusChanged; 

    public void Connect(
        TimeSpan minimumPollPeriod,
        ExtendedPollProfileOptions pollOptions)
    {
        if(IsConnected)
            return;
        lock (connectLock)
        {
            if(IsConnected)
                return;

            ConfigureSerialPort();
            serialPortCommunicator!.Start();

            SendAssociationRequest(minimumPollPeriod, pollOptions);
            var associationResult = WaitForAssociationCommandMessage(AssociationCommandType.AssociationAccepted, AssociationCommandType.Refuse);
            if (associationResult.SessionHeader.Type == AssociationCommandType.Refuse)
            {
                serialPortCommunicator.Stop();
                throw new Exception("Monitor refused association");
            }
            var mdsCreateEventReport = WaitForDataExportCommandMessage(RemoteOperationType.Invoke, DataExportCommandType.ConfirmedEventReport);
            SendMdsCreateEventResult(mdsCreateEventReport);
            IsConnected = true;
            Log("Connected");
            ConnectionStatusChanged?.Invoke(this, MonitorConnectionChangeEventType.Connected);
        }
    }

    private void ReportNewMessage(
        object? sender,
        ICommandMessage message)
    {
        NewMessage?.Invoke(this, message);
    }

    private void ConfigureSerialPort()
    {
        serialPort = settings.UseSimulatedSerialPort
            ? settings.SimulatedSerialPort!
            : new PhysicalSerialPort(
                settings.SerialPortName,
                settings.SerialPortBaudRate,
                Parity.None,
                8,
                StopBits.One);
        serialPort.Encoding = Encoding.Unicode;
        serialPort.Open();
        serialPortCommunicator = new SerialPortCommunicator(serialPort, settings.MessageRetentionPeriod, nameof(PhilipsIntellivueClient));
        serialPortCommunicator.NewMessage += ReportNewMessage;
        serialPortCommunicator.ConnectionStatusChanged += OnConnectionStatusChanged;
    }

    private void OnConnectionStatusChanged(
        object? sender,
        MonitorConnectionChangeEventType connectionChangeEventType)
    {
        ConnectionStatusChanged?.Invoke(sender, connectionChangeEventType);
    }

    /// <summary>
    /// Send Association Request to monitor
    /// </summary>
    /// <returns>An association response or association refuse message</returns>
    private void SendAssociationRequest(
        TimeSpan minimumPollPeriod,
        ExtendedPollProfileOptions pollOptions)
    {
        var associationRequestMessage = messageCreator.CreateAssociationRequest(
            minimumPollPeriod,
            pollOptions);
        serialPortCommunicator!.Enqueue(associationRequestMessage);
    }

    private AssociationCommandMessage WaitForAssociationCommandMessage(
        params AssociationCommandType[] commandTypes)
    {
        var waitRequest = new WaitForRequest<ICommandMessage>(
            Guid.NewGuid(),
            message =>
            {
                if (message is not AssociationCommandMessage associationCommandMessage)
                    return false;
                return commandTypes.Contains(associationCommandMessage.SessionHeader.Type);
            });
        serialPortCommunicator!.WaitForMessage(waitRequest);
        if(!waitRequest.WaitHandle.WaitOne(TimeSpan.FromSeconds(10)))
            throw new TimeoutException($"Didn't receive Association command message of type {string.Join('|', commandTypes)}");
        return (AssociationCommandMessage)waitRequest.MatchingItem!;
    }

    private void SendMdsCreateEventResult(
        DataExportCommandMessage mdsCreateEventReport)
    {
        var invokeId = mdsCreateEventReport.RemoteOperationData.InvokeId;
        var managedObject = ((MdsCreateInfo)((EventReportCommand)((RemoteOperationInvoke)mdsCreateEventReport.RemoteOperationData).Data).Data).ManagedObject;
        var mdsCreateEventReportResult = messageCreator.CreateMdsCreateEventResult(Constants.DefaultPresentationContextId, invokeId, managedObject);
        serialPortCommunicator!.Enqueue(mdsCreateEventReportResult);
    }

    private DataExportCommandMessage WaitForDataExportCommandMessage(
        RemoteOperationType operationType,
        DataExportCommandType? commandType = null,
        OIDType? actionType = null)
    {
        if (operationType != RemoteOperationType.Error && !commandType.HasValue)
            throw new ArgumentException("Command type must be specified, except when waiting for an error message");
        if(operationType != RemoteOperationType.Error && commandType == DataExportCommandType.ConfirmedAction && !actionType.HasValue)
            throw new ArgumentException("Action type must be specified, except when waiting for an error message");

        var waitRequest = new WaitForRequest<ICommandMessage>(
            Guid.NewGuid(),
            message =>
            {
                if (message is not DataExportCommandMessage dataExportCommandMessage)
                    return false;
                if (dataExportCommandMessage.RemoteOperationHeader.Type != operationType)
                    return false;
                if (operationType == RemoteOperationType.Error)
                    return true;
                var remoteOperation = (IRemoteOperation)dataExportCommandMessage.RemoteOperationData;
                if (remoteOperation.CommandType != commandType)
                    return false;
                if (commandType != DataExportCommandType.ConfirmedAction)
                    return true;
                switch (operationType)
                {
                    case RemoteOperationType.Invoke:
                        var actionCommand = (ActionCommand)((RemoteOperationInvoke)remoteOperation).Data;
                        return actionCommand.ActionType == actionType;
                    case RemoteOperationType.Result:
                    case RemoteOperationType.LinkedResult:
                        var actionResult = (ActionResultCommand)((RemoteOperationResult)remoteOperation).Data;
                        return actionResult.ActionType == actionType;
                }
                return false;
            });
        serialPortCommunicator!.WaitForMessage(waitRequest);
        if(!waitRequest.WaitHandle.WaitOne(TimeSpan.FromSeconds(10)))
            throw new TimeoutException($"Didn't receive Data Export command message with operation {operationType} and command {commandType}");
        return (DataExportCommandMessage)waitRequest.MatchingItem!;
    }

    public void StartPolling()
    {
        alertPollTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(settings.PollMode == PollMode.Extended ? 30 : 10));
        numericsPollTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(settings.PollMode == PollMode.Extended ? 10 : 1));
        Log("Started polling for alerts and numerics");
    }

    private void SendAlertPollRequest(
        object? state)
    {
        ICommandMessage pollMessage = settings.PollMode switch {
            PollMode.Single => messageCreator.CreateSinglePollRequest(
                Constants.DefaultPresentationContextId,
                PollObjectTypes.Alerts,
            PollAttributeGroups.Alerts.AlertMonitor),
            PollMode.Extended => messageCreator.CreateExtendedPollRequest(
                Constants.DefaultPresentationContextId,
                PollObjectTypes.Alerts,
            PollAttributeGroups.Alerts.AlertMonitor,
                TimeSpan.FromSeconds(60)),
            _ => throw new ArgumentOutOfRangeException(nameof(settings.PollMode))
        };
        serialPortCommunicator!.Enqueue(pollMessage);
    }

    private void SendNumericsPollRequest(
        object? state)
    {
        ICommandMessage pollMessage = settings.PollMode switch {
            PollMode.Single => messageCreator.CreateSinglePollRequest(
                Constants.DefaultPresentationContextId,
                PollObjectTypes.Numerics,
                PollAttributeGroups.Numerics.MetricObservedValue),
            PollMode.Extended => messageCreator.CreateExtendedPollRequest(
                Constants.DefaultPresentationContextId,
                PollObjectTypes.Numerics,
                PollAttributeGroups.Numerics.MetricObservedValue,
                TimeSpan.FromSeconds(60)),
            _ => throw new ArgumentOutOfRangeException(nameof(settings.PollMode))
        };
        serialPortCommunicator!.Enqueue(pollMessage);
    }

    public void SendPatientDemographicsRequest()
    {
        var patientDemographicsRequestMessage = messageCreator.CreateSinglePollRequest(
            Constants.DefaultPresentationContextId,
            PollObjectTypes.PatientDemographics,
            PollAttributeGroups.PatientDemographics.All);
        serialPortCommunicator!.Enqueue(patientDemographicsRequestMessage);
    }

    public void Disconnect()
    {
        if(!IsConnected)
            return;
        lock (connectLock)
        {
            if(!IsConnected)
                return;

            IsConnected = false;
            ConnectionStatusChanged?.Invoke(this, MonitorConnectionChangeEventType.Disconnected);
            StopPolling();

            SendAssociationReleaseRequest();
            WaitForAssociationCommandMessage(AssociationCommandType.Released);

            serialPortCommunicator?.Stop();
            serialPort?.Close();
            Log("Disconnected");
        }
    }

    public void StopPolling()
    {
        alertPollTimer.Change(Timeout.Infinite, Timeout.Infinite);
        numericsPollTimer.Change(Timeout.Infinite, Timeout.Infinite);
        Log("Stopped polling for alerts and numerics");
    }

    private void SendAssociationReleaseRequest()
    {
        serialPortCommunicator!.Enqueue(messageCreator.CreateAssociationReleaseRequest());
    }

    public void Dispose()
    {
        Disconnect();
    }

    public async ValueTask DisposeAsync()
    {
        Dispose();
    }

    public void Log(string message) => Console.WriteLine($"{nameof(PhilipsIntellivueClient)} - {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} - {message}");
}