using System.Collections.Concurrent;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.Simulation;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class SimulatedPhilipsIntellivueMonitor : IDisposable
{
    private readonly SimulatedIoDevice serialPort;
    private readonly PhilipsIntellivueCommunicator philipsIntellivueCommunicator;
    private readonly CommandMessageCreator messageCreator = new();
    private ushort nextInvokeId;
    private readonly RelativeToAbsoluteTimeTranslator timeTranslator = new(0, DateTime.UtcNow);
    private readonly ConcurrentDictionary<PollObjectTypeAndAttributeGroup, PerioidPollReplier> periodicPollRepliers = new();
    private readonly Timer connectionTimeoutTimer;
    private TimeSpan minimumPollPeriod = TimeSpan.FromSeconds(60);
    private TimeSpan ConnectionTimeoutLength => minimumPollPeriod < TimeSpan.FromSeconds(3.3) ? TimeSpan.FromSeconds(10) 
        : minimumPollPeriod < TimeSpan.FromSeconds(43) ? 3 * minimumPollPeriod 
        : TimeSpan.FromSeconds(130);
    private readonly MonitorDataGenerator monitorDataGenerator;

    public SimulatedPhilipsIntellivueMonitor(
        SimulatedIoDevice serialPort)
    {
        this.serialPort = serialPort;
        philipsIntellivueCommunicator = new PhilipsIntellivueCommunicator(serialPort, TimeSpan.FromSeconds(10), nameof(SimulatedPhilipsIntellivueMonitor));
        monitorDataGenerator = new MonitorDataGenerator();
        connectionTimeoutTimer = new Timer(
            AbortConnection,
            null,
            Timeout.Infinite,
            Timeout.Infinite);
    }

    public bool IsListening => philipsIntellivueCommunicator.IsListening;
    public Association? CurrentAssociation { get; private set; }

    public void Start()
    {
        if(IsListening)
            return;
        philipsIntellivueCommunicator.Start();
        philipsIntellivueCommunicator.NewMessage += ProcessMessage;
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
                                philipsIntellivueCommunicator.Enqueue(refuseAssociationMessage);
                            }
                            else
                            {
                                // TODO: Set minimum poll time
                                var associationAcceptMessage = messageCreator.CreateAssociationAccept(associationCommandMessage);
                                philipsIntellivueCommunicator.Enqueue(associationAcceptMessage);
                                CurrentAssociation = new Association
                                {
                                    PresentationContextId = Constants.DefaultPresentationContextId
                                };

                                var eventTime = timeTranslator.GetCurrentRelativeTime();
                                var mdsCreateEventMessage = messageCreator.CreateMdsCreateEvent(CurrentAssociation.PresentationContextId, nextInvokeId++, eventTime);
                                philipsIntellivueCommunicator.Enqueue(mdsCreateEventMessage);
                            }
                            break;
                        case AssociationCommandType.RequestRelease:
                        {
                            ReleaseAssociation();
                            break;
                        }
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
                                    {
#if DEBUG
                                        Log("Received MDS Create Event report");
#endif
                                    }
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

    private void ReleaseAssociation()
    {
        if(CurrentAssociation == null)
            return;
        CurrentAssociation = null;
        ClearPeriodicPollReplies();
        var releaseResponseMessage = messageCreator.CreateAssociationReleaseResponse();
        philipsIntellivueCommunicator.Enqueue(releaseResponseMessage);
    }

    private void HandleExtendedPollRequest(
        ushort invokeId,
        ExtendedPollMdiDataRequest pollRequest)
    {
        if(CurrentAssociation == null)
            return;

        SendExtendedPollReply(
            pollRequest.ObjectType,
            pollRequest.AttributeGroup,
            invokeId,
            0, // Even if periodic data are currently send, this message is out of sequence and intentionally has sequence number 0, as specified by the protocol.
            pollRequest);

        var periodicPollAttribute = pollRequest.Attributes.Values.Find(attribute => attribute.AttributeId == (ushort)OIDType.NOM_ATTR_TIME_PD_POLL);
        var isPeriodic = periodicPollAttribute != null;
        if (isPeriodic)
        {
            var key = new PollObjectTypeAndAttributeGroup(pollRequest.ObjectType, pollRequest.AttributeGroup);
            var inactivePollRepliers = periodicPollRepliers.Where(kvp => !kvp.Value.IsActive).Select(kvp => kvp.Key).ToList();
            foreach (var objectTypeAndAttributeGroup in inactivePollRepliers)
            {
                if(periodicPollRepliers.TryRemove(objectTypeAndAttributeGroup, out var inactivePollReplier))
                    inactivePollReplier.Dispose();
            }
            if (!periodicPollRepliers.TryGetValue(key, out var perioidPollReplier))
            {
                perioidPollReplier = new PerioidPollReplier(
                    CurrentAssociation,
                    invokeId,
                    pollRequest,
                    minimumPollPeriod,
                    philipsIntellivueCommunicator,
                    timeTranslator,
                    messageCreator,
                    monitorDataGenerator);
                periodicPollRepliers.TryAdd(key, perioidPollReplier);
            }
            else
            {
                perioidPollReplier.RenewPoll(pollRequest);
            }
        }
    }

    private void SendExtendedPollReply(
        NomenclatureReference objectType,
        OIDType attributeGroup,
        ushort invokeId,
        ushort sequenceNumber,
        ExtendedPollMdiDataRequest pollRequest)
    {
        if(CurrentAssociation == null)
            return;

        var observations = monitorDataGenerator.Generate(objectType, attributeGroup);
        var replyMessage = messageCreator.CreateExtendedPollReply(
            CurrentAssociation.PresentationContextId,
            invokeId,
            sequenceNumber,
            pollRequest,
            timeTranslator.GetCurrentRelativeTime(),
            observations);
        philipsIntellivueCommunicator.Enqueue(replyMessage);
    }

    private void HandlePollRequest(
        ushort invokeId,
        PollMdiDataRequest pollMdiDataRequest)
    {
        if(CurrentAssociation == null)
            return;

        var attributes = monitorDataGenerator.Generate(pollMdiDataRequest.ObjectType, pollMdiDataRequest.AttributeGroup);
        var replyMessage = messageCreator.CreatePollReply(
            CurrentAssociation.PresentationContextId,
            invokeId,
            pollMdiDataRequest,
            timeTranslator.GetCurrentRelativeTime(),
            attributes);
        philipsIntellivueCommunicator.Enqueue(replyMessage);
    }

    private void AbortConnection(
        object? state)
    {
        if(CurrentAssociation == null)
            return;
        var abortMessage = messageCreator.CreateAssociationAbort();
        philipsIntellivueCommunicator.Enqueue(abortMessage);
        ClearPeriodicPollReplies();
        CurrentAssociation = null;
    }

    private void ClearPeriodicPollReplies()
    {
        foreach (var pollReplyTimer in periodicPollRepliers.Values)
        {
            pollReplyTimer.Dispose();
        }
        periodicPollRepliers.Clear();
    }

    private void Log(string message) => Console.WriteLine($"{nameof(SimulatedPhilipsIntellivueMonitor)} - {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}: {message}");

    public void Stop()
    {
        ClearPeriodicPollReplies();
        AbortConnection(null);
        philipsIntellivueCommunicator.Stop();
        philipsIntellivueCommunicator.NewMessage -= ProcessMessage;
    }

    public void Dispose()
    {
        Stop();
        philipsIntellivueCommunicator.Dispose();
    }


    private class PollObjectTypeAndAttributeGroup : IEquatable<PollObjectTypeAndAttributeGroup>
    {
        public PollObjectTypeAndAttributeGroup(
            NomenclatureReference objectType,
            OIDType attributeGroup)
        {
            ObjectType = objectType;
            AttributeGroup = attributeGroup;
        }

        public NomenclatureReference ObjectType { get; }
        public OIDType AttributeGroup { get; }

        public bool Equals(
            PollObjectTypeAndAttributeGroup? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return ObjectType.Equals(other.ObjectType) && AttributeGroup == other.AttributeGroup;
        }

        public override bool Equals(
            object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PollObjectTypeAndAttributeGroup)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ObjectType, (int)AttributeGroup);
        }

        public static bool operator ==(
            PollObjectTypeAndAttributeGroup? left,
            PollObjectTypeAndAttributeGroup? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(
            PollObjectTypeAndAttributeGroup? left,
            PollObjectTypeAndAttributeGroup? right)
        {
            return !Equals(left, right);
        }
    }
}

public class Association
{
    public ushort PresentationContextId { get; set; }
}