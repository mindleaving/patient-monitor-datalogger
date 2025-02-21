using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class SimulatedPhilipsIntellivueMonitor : IDisposable
{
    private readonly SimulatedSerialPort serialPort;
    private readonly SerialPortCommunicator serialPortCommunicator;
    private readonly CommandMessageCreator messageCreator = new();
    private ushort nextInvokeId;
    private readonly DateTime startTime;
    private readonly Timer pollReplyTimer;
    private ExtendPollSettings? extendedPollSettings;
    private readonly Timer connectionTimeoutTimer;
    private TimeSpan minimumPollPeriod = TimeSpan.FromSeconds(60);
    private TimeSpan ConnectionTimeoutLength => minimumPollPeriod < TimeSpan.FromSeconds(3.3) ? TimeSpan.FromSeconds(10) 
        : minimumPollPeriod < TimeSpan.FromSeconds(43) ? 3 * minimumPollPeriod 
        : TimeSpan.FromSeconds(130);
    private DateTime? lastPollRequestTime;
    private readonly MonitorDataGenerator pollDataGenerator;

    public SimulatedPhilipsIntellivueMonitor(
        SimulatedSerialPort serialPort)
    {
        this.serialPort = serialPort;
        serialPortCommunicator = new SerialPortCommunicator(serialPort, TimeSpan.FromSeconds(10), nameof(SimulatedPhilipsIntellivueMonitor));
        startTime = DateTime.UtcNow;
        pollDataGenerator = new MonitorDataGenerator();
        pollReplyTimer = new Timer(
            SendPollReply,
            null,
            Timeout.Infinite,
            Timeout.Infinite);
        connectionTimeoutTimer = new Timer(
            AbortConnection,
            null,
            Timeout.Infinite,
            Timeout.Infinite);
    }

    public bool IsListening => serialPortCommunicator.IsListening;
    public Association? CurrentAssociation { get; private set; }

    public void Start()
    {
        serialPortCommunicator.Start();
        serialPortCommunicator.NewMessage += ProcessMessage;
    }

    private void ProcessMessage(
        object? sender,
        ICommandMessage message)
    {
        // Reset timeout
        connectionTimeoutTimer.Change(ConnectionTimeoutLength, Timeout.InfiniteTimeSpan);

        try
        {
            switch (message)
            {
                case AssociationCommandMessage associationCommandMessage:
                    switch (associationCommandMessage.SessionHeader.Type)
                    {
                        case AssociationCommandType.RequestAssociation:
                            if (CurrentAssociation != null)
                            {
                                var refuseAssociationMessage = messageCreator.CreateAssociationRefusal();
                                serialPortCommunicator.Enqueue(refuseAssociationMessage);
                            }
                            else
                            {
                                // TODO: Set minimum poll time
                                var associationAcceptMessage = messageCreator.CreateAssociationAccept(associationCommandMessage);
                                serialPortCommunicator.Enqueue(associationAcceptMessage);
                                CurrentAssociation = new Association
                                {
                                    PresentationContextId = Constants.DefaultPresentationContextId
                                };

                                var eventTime = GetCurrentRelativeTime();
                                var mdsCreateEventMessage = messageCreator.CreateMdsCreateEvent(CurrentAssociation.PresentationContextId, nextInvokeId++, eventTime);
                                serialPortCommunicator.Enqueue(mdsCreateEventMessage);
                            }
                            break;
                        case AssociationCommandType.RequestRelease:
                            var releaseResponseMessage = messageCreator.CreateAssociationReleaseResponse();
                            serialPortCommunicator.Enqueue(releaseResponseMessage);
                            break;
                    }
                    break;
                case DataExportCommandMessage dataExportCommandMessage:
                    switch (dataExportCommandMessage.RemoteOperationData)
                    {
                        case RemoteOperationError remoteOperationError:
                            Log($"Received {remoteOperationError.ErrorValue} error");
                            break;
                        case RemoteOperationInvoke remoteOperationInvoke:
                            switch (remoteOperationInvoke.Data)
                            {
                                case ActionCommand actionCommand:
                                    switch (actionCommand.Data)
                                    {
                                        case ExtendedPollMdiDataRequest extendedPollMdiDataRequest:
                                            HandleExtendedPollRequest(remoteOperationInvoke.InvokeId, extendedPollMdiDataRequest);
                                            break;
                                        case PollMdiDataRequest pollMdiDataRequest:
                                            HandlePollRequest(remoteOperationInvoke.InvokeId, pollMdiDataRequest);
                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                    break;
                                case EventReportCommand eventReportCommand:
                                    break;
                                case GetCommand getCommand:
                                    break;
                                case SetCommand setCommand:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case RemoteOperationLinkedResult remoteOperationLinkedResult:
                            break;
                        case RemoteOperationResult remoteOperationResult:
                            switch (remoteOperationResult.Data)
                            {
                                case ActionResultCommand actionResultCommand:
                                    break;
                                case EventReportResultCommand eventReportResultCommand:
                                    if(eventReportResultCommand.EventType == OIDType.NOM_NOTI_MDS_CREAT)
                                        Log("Received MDS Create Event report");
                                    break;
                                case GetResultCommand getResultCommand:
                                    break;
                                case SetResultCommand setResultCommand:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    Log($"Received unknown message of type {message.GetType().Name}");
                    return; // Ignore
            }
        }
        catch (Exception e)
        {
            Log($"Couldn't process message: {e.Message}");
        }
    }

    private void HandleExtendedPollRequest(
        ushort invokeId,
        ExtendedPollMdiDataRequest pollRequest)
    {
        var periodicPollAttribute = pollRequest.Attributes.Values.Find(attribute => attribute.AttributeId == (ushort)OIDType.NOM_ATTR_TIME_PD_POLL);
        var isPeriodic = periodicPollAttribute != null;
        if (!isPeriodic)
        {
            HandlePollRequest(invokeId, pollRequest);
            return;
        }
        //var binaryReader = new BigEndianBinaryReader(new MemoryStream(periodicPollAttribute!.AttributeValue));
        //var pollPeriod = PollDataRequestPeriod.Read(binaryReader);

        lastPollRequestTime = DateTime.UtcNow;
        extendedPollSettings = new ExtendPollSettings(invokeId, pollRequest);
        var resultPeriod = DetermineExtendedPollResultPeriod(pollRequest.ObjectType);
        pollReplyTimer.Change(TimeSpan.Zero, resultPeriod); // Send first response immediately
    }

    private TimeSpan DetermineExtendedPollResultPeriod(
        NomenclatureReference objectType)
    {
        if (objectType == PollObjectTypes.Alerts)
        {
            return TimeSpan.FromSeconds(1);
        }

        if (objectType == PollObjectTypes.Numerics)
        {
            return TimeSpan.FromSeconds(1);
        }

        if (objectType == PollObjectTypes.Waves)
        {
            return TimeSpan.FromMilliseconds(256);
        }

        throw new ArgumentOutOfRangeException(nameof(objectType), $"Cannot determine result period for {objectType}. It is not supported for extended polling.");
    }

    private void SendPollReply(
        object? state)
    {
        if(extendedPollSettings == null)
            return;
        if(CurrentAssociation == null)
            return;
        if(DateTime.UtcNow - lastPollRequestTime > minimumPollPeriod)
        {
            pollReplyTimer.Change(Timeout.Infinite, Timeout.Infinite);
            return;
        }

        var attributes = pollDataGenerator.Generate(extendedPollSettings.PollRequest.ObjectType, extendedPollSettings.PollRequest.AttributeGroup);
        var replyMessage = messageCreator.CreateExtendedPollReply(
            CurrentAssociation.PresentationContextId,
            extendedPollSettings.InvokeId,
            extendedPollSettings.SequenceNumber++,
            extendedPollSettings.PollRequest,
            GetCurrentRelativeTime(),
            attributes);
        serialPortCommunicator.Enqueue(replyMessage);
    }

    private void HandlePollRequest(
        ushort invokeId,
        PollMdiDataRequest pollMdiDataRequest)
    {
        if(CurrentAssociation == null)
            return;

        var attributes = pollDataGenerator.Generate(pollMdiDataRequest.ObjectType, pollMdiDataRequest.AttributeGroup);
        var replyMessage = messageCreator.CreatePollReply(
            CurrentAssociation.PresentationContextId,
            invokeId,
            pollMdiDataRequest,
            GetCurrentRelativeTime(),
            attributes);
        serialPortCommunicator.Enqueue(replyMessage);
    }

    private void AbortConnection(
        object? state)
    {
        var abortMessage = messageCreator.CreateAssociationAbort();
        serialPortCommunicator.Enqueue(abortMessage);
        pollReplyTimer.Change(Timeout.Infinite, Timeout.Infinite);
        CurrentAssociation = null;
    }

    private void Log(string message) => Console.WriteLine($"{nameof(SimulatedPhilipsIntellivueMonitor)} - {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}: {message}");

    public void Stop()
    {
        serialPortCommunicator.Stop();
    }

    public void Dispose()
    {
        serialPortCommunicator.Dispose();
    }

    private RelativeTime GetCurrentRelativeTime()
    {
        var now = DateTime.UtcNow;
        var secondsSinceStart = (now - startTime).TotalSeconds;
        return new(TimeSpan.FromSeconds(secondsSinceStart));
    }

    private class ExtendPollSettings
    {
        public ExtendPollSettings(
            ushort invokeId,
            ExtendedPollMdiDataRequest pollRequest)
        {
            InvokeId = invokeId;
            PollRequest = pollRequest;
        }

        public ushort InvokeId { get; }
        public ExtendedPollMdiDataRequest PollRequest { get; }
        public ushort SequenceNumber { get; set; }
    }

    public class Association
    {
        public ushort PresentationContextId { get; set; }
    }
}