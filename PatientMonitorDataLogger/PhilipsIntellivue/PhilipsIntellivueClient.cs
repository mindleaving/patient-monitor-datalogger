using System.Data;
using System.IO.Ports;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class PhilipsIntellivueClient : IDisposable
{
    private readonly PhilipsIntellivueClientSettings settings;
    private IODevice? ioDevice;
    private PhilipsIntellivueCommunicator? protocolCommunicator;
    private readonly CommandMessageCreator messageCreator = new();
    private readonly object connectLock = new();
    private readonly Timer alertPollTimer;
    private readonly Timer numericsPollTimer;
    private readonly Timer wavesPollTimer;

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
        wavesPollTimer = new Timer(
            SendWavesPollRequest,
            null,
            Timeout.Infinite,
            Timeout.Infinite);
    }

    private bool IsConfigured { get; set; }
    public bool IsConnected { get; private set; }
    public bool IsListening => protocolCommunicator?.IsListening ?? false;
    public event EventHandler<ICommandMessage>? NewMessage;
    public event EventHandler<ConnectionState>? ConnectionStatusChanged; 

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
            ioDevice!.Open();
            protocolCommunicator!.Start();

            try
            {
                SendAssociationRequest(minimumPollPeriod, pollOptions);
                var associationResult = WaitForAssociationCommandMessage(AssociationCommandType.AssociationAccepted, AssociationCommandType.Refuse);
                if (associationResult.SessionHeader.Type == AssociationCommandType.Refuse)
                    throw new Exception("Monitor refused association");
            }
            catch
            {
                ioDevice!.Close();
                protocolCommunicator.Stop();
                throw;
            }

            try
            {
                var mdsCreateEventReport = WaitForDataExportCommandMessage(
                    TimeSpan.FromSeconds(10),
                    RemoteOperationType.Invoke,
                    DataExportCommandType.ConfirmedEventReport);
                SendMdsCreateEventResult(mdsCreateEventReport);
            }
            catch
            {
                ioDevice!.Close();
                protocolCommunicator.Stop();
                throw;
            }
            IsConnected = true;
            Log("Connected");
            ConnectionStatusChanged?.Invoke(this, ConnectionState.Open);
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
        if(IsConfigured)
            return;
        ioDevice = settings.UseSimulatedSerialPort
            ? settings.SimulatedSerialPort!
            : new PhysicalSerialPort(
                settings.SerialPortName,
                settings.SerialPortBaudRate,
                Parity.None,
                8,
                StopBits.One);
        protocolCommunicator = new PhilipsIntellivueCommunicator(ioDevice, settings.MessageRetentionPeriod, nameof(PhilipsIntellivueClient));
        protocolCommunicator.NewMessage += ReportNewMessage;
        protocolCommunicator.ConnectionStatusChanged += OnConnectionStatusChanged;
        IsConfigured = true;
    }

    private void OnConnectionStatusChanged(
        object? sender,
        ConnectionState connectionStatus)
    {
        ConnectionStatusChanged?.Invoke(sender, connectionStatus);
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
        Send(associationRequestMessage);
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
        protocolCommunicator!.WaitForMessage(waitRequest);
        if(!waitRequest.WaitHandle.WaitOne(TimeSpan.FromSeconds(5)))
            throw new TimeoutException($"Didn't receive Association command message of type {string.Join('|', commandTypes)}");
        return (AssociationCommandMessage)waitRequest.MatchingItem!;
    }

    private void SendMdsCreateEventResult(
        DataExportCommandMessage mdsCreateEventReport)
    {
        var invokeId = mdsCreateEventReport.RemoteOperationData.InvokeId;
        var managedObject = ((MdsCreateInfo)((EventReportCommand)((RemoteOperationInvoke)mdsCreateEventReport.RemoteOperationData).Data).Data).ManagedObject;
        var mdsCreateEventReportResult = messageCreator.CreateMdsCreateEventResult(Constants.DefaultPresentationContextId, invokeId, managedObject);
        Send(mdsCreateEventReportResult);
    }

    private DataExportCommandMessage WaitForDataExportCommandMessage(
        TimeSpan maxWaitTime,
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
        protocolCommunicator!.WaitForMessage(waitRequest);
        if(!waitRequest.WaitHandle.WaitOne(maxWaitTime))
            throw new TimeoutException($"Didn't receive Data Export command message with operation {operationType} and command {commandType}");
        return (DataExportCommandMessage)waitRequest.MatchingItem!;
    }

    public void StartPolling(
        PatientMonitorDataSettings monitorDataSettings)
    {
        if(monitorDataSettings.IncludeAlerts)
            alertPollTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(settings.PollMode == PollMode.Extended ? 30 : 10));
        if(monitorDataSettings.IncludeNumerics)
            numericsPollTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(settings.PollMode == PollMode.Extended ? 10 : 1));
        if(monitorDataSettings.IncludeWaves)
            wavesPollTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(settings.PollMode == PollMode.Extended ? 10 : 1));
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
        Send(pollMessage);
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
        Send(pollMessage);
    }

    private void SendWavesPollRequest(
        object? state)
    {
        ICommandMessage pollMessage = settings.PollMode switch {
            PollMode.Single => messageCreator.CreateSinglePollRequest(
                Constants.DefaultPresentationContextId,
                PollObjectTypes.Waves,
                PollAttributeGroups.Waves.MetricObservedValue),
            PollMode.Extended => messageCreator.CreateExtendedPollRequest(
                Constants.DefaultPresentationContextId,
                PollObjectTypes.Waves,
                PollAttributeGroups.Waves.MetricObservedValue,
                TimeSpan.FromSeconds(30)),
            _ => throw new ArgumentOutOfRangeException(nameof(settings.PollMode))
        };
        Send(pollMessage);
    }

    public void SendPatientDemographicsRequest()
    {
        var patientDemographicsRequestMessage = messageCreator.CreateSinglePollRequest(
            Constants.DefaultPresentationContextId,
            PollObjectTypes.PatientDemographics,
            PollAttributeGroups.PatientDemographics.All);
        Send(patientDemographicsRequestMessage);
    }

    public void SetWavePriorityList(
        ICollection<Labels> waveLabels)
    {
        var setPriorityListRequest = messageCreator.CreateSetPriorityListRequest(
            Constants.DefaultPresentationContextId,
            MonitorDataType.Wave,
            new Models.List<TextId>(waveLabels.Select(label => new TextId(label)).ToList()));
        Send(setPriorityListRequest);
    }

    public void Send(ICommandMessage message) => protocolCommunicator!.Enqueue(message);

    public async Task<DataExportCommandMessage> SendAndWait(
        DataExportCommandMessage request,
        TimeSpan maxWaitTime)
    {
        if (request.RemoteOperationData is not RemoteOperationInvoke invokeOperation)
            throw new ArgumentException("Request must be an invoke-command");
        Send(request);
        OIDType? actionType = invokeOperation.Data is ActionCommand actionCommand ? actionCommand.ActionType : null;

        return await Task.Run(() => WaitForDataExportCommandMessage(
            maxWaitTime,
            request.RemoteOperationHeader.Type,
            invokeOperation.CommandType,
            actionType));
    }

    public void StopPolling()
    {
        alertPollTimer.Change(Timeout.Infinite, Timeout.Infinite);
        numericsPollTimer.Change(Timeout.Infinite, Timeout.Infinite);
        wavesPollTimer.Change(Timeout.Infinite, Timeout.Infinite);
        Log("Stopped polling for alerts and numerics");
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
            try
            {
                ConnectionStatusChanged?.Invoke(this, ConnectionState.Closed);
            }
            catch
            {
                // Ignore
            }

            try
            {
                StopPolling();
            }
            catch
            {
                // Ignore
            }

            try
            {
                SendAssociationReleaseRequest();
                WaitForAssociationCommandMessage(AssociationCommandType.Released);
            }
            catch
            {
                // Ignore
            }

            try
            {
                ioDevice?.Close();
                protocolCommunicator?.Stop();
            }
            catch
            {
                // Ignore
            }
            Log("Disconnected");
        }
    }

    private void SendAssociationReleaseRequest()
    {
        protocolCommunicator!.Enqueue(messageCreator.CreateAssociationReleaseRequest());
    }

    public void Dispose()
    {
        Disconnect();
        if(protocolCommunicator != null)
        {
            protocolCommunicator.NewMessage -= ReportNewMessage;
            protocolCommunicator.ConnectionStatusChanged -= OnConnectionStatusChanged;
        }
    }

    public void Log(string message) => Console.WriteLine($"{nameof(PhilipsIntellivueClient)} - {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} - {message}");
}