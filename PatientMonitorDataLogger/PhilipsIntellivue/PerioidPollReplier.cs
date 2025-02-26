using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class PerioidPollReplier : IDisposable, IAsyncDisposable
{
    private readonly Timer pollReplyTimer;
    private ExtendPollSettings extendedPollSettings;
    private DateTime? lastPollRequestTime;
    private readonly TimeSpan minimumPollPeriod;
    private readonly Association association;
    private readonly SerialPortCommunicator serialPortCommunicator;
    private readonly RelativeToAbsoluteTimeTranslator timeTranslator;
    private readonly CommandMessageCreator messageCreator;
    private readonly MonitorDataGenerator monitorDataGenerator;
    private readonly TimeSpan replyPeriod;

    public PerioidPollReplier(
        Association association,
        ushort invokeId,
        ExtendedPollMdiDataRequest pollRequest,
        TimeSpan minimumPollPeriod,
        SerialPortCommunicator serialPortCommunicator,
        RelativeToAbsoluteTimeTranslator timeTranslator,
        CommandMessageCreator messageCreator,
        MonitorDataGenerator monitorDataGenerator)
    {
        this.association = association;
        this.minimumPollPeriod = minimumPollPeriod;
        this.serialPortCommunicator = serialPortCommunicator;
        this.timeTranslator = timeTranslator;
        this.messageCreator = messageCreator;
        this.monitorDataGenerator = monitorDataGenerator;
        extendedPollSettings = new ExtendPollSettings(invokeId, pollRequest);
        // TODO: Handle max poll period
        //if (!TryGetPollPeriod(pollRequest, out var pollPeriod))
        //    pollPeriod = TimeSpan.FromSeconds(30);
        replyPeriod = DetermineExtendedPollResultPeriod(pollRequest.ObjectType);
        pollReplyTimer = new Timer(
            SendPeriodicPollReply,
            null,
            replyPeriod,
            replyPeriod);
        lastPollRequestTime = DateTime.UtcNow;
    }

    public bool IsActive { get; private set; }

    public void RenewPoll(
        ExtendedPollMdiDataRequest pollRequest)
    {
        lastPollRequestTime = DateTime.UtcNow;
        extendedPollSettings = new ExtendPollSettings(extendedPollSettings.InvokeId, pollRequest);
        if (!IsActive)
        {
            pollReplyTimer.Change(replyPeriod, replyPeriod);
            IsActive = true;
        }
    }

    private void SendPeriodicPollReply(
        object? state)
    {
        if(DateTime.UtcNow - lastPollRequestTime > minimumPollPeriod)
        {
            IsActive = false;
            pollReplyTimer.Change(Timeout.Infinite, Timeout.Infinite);
            extendedPollSettings.SequenceNumber = 1;
            return;
        }

        SendExtendedPollReply(
            extendedPollSettings.PollRequest.ObjectType,
            extendedPollSettings.PollRequest.AttributeGroup,
            extendedPollSettings.InvokeId, extendedPollSettings.SequenceNumber++,
            extendedPollSettings.PollRequest);
    }

    private void SendExtendedPollReply(
        NomenclatureReference objectType,
        OIDType attributeGroup,
        ushort invokeId,
        ushort sequenceNumber,
        ExtendedPollMdiDataRequest pollRequest)
    {
        var observations = monitorDataGenerator.Generate(objectType, attributeGroup);
        var replyMessage = messageCreator.CreateExtendedPollReply(
            association.PresentationContextId,
            invokeId,
            sequenceNumber,
            pollRequest,
            timeTranslator.GetCurrentRelativeTime(),
            observations);
        serialPortCommunicator.Enqueue(replyMessage);
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

    public void Dispose()
    {
        pollReplyTimer.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await pollReplyTimer.DisposeAsync();
    }

    private class ExtendPollSettings
    {
        public ExtendPollSettings(
            ushort invokeId,
            ExtendedPollMdiDataRequest pollRequest)
        {
            InvokeId = invokeId;
            PollRequest = pollRequest;
            SequenceNumber = 1;
        }

        public ushort InvokeId { get; }
        public ExtendedPollMdiDataRequest PollRequest { get; }
        public ushort SequenceNumber { get; set; }
    }
}