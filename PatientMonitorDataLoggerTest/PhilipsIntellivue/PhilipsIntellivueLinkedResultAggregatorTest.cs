using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.DataProcessing;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest.PhilipsIntellivue;

public class PhilipsIntellivueLinkedResultAggregatorTest
{
    [Test]
    public void NotLinkedResultIsImmediatelyReleased()
    {
        var message = CreateUnlinkedMessage();
        var sut = new PhilipsIntellivueLinkedResultAggregator();
        var messageBundles = sut.TestMessageAndAggregateOrRelease(message).ToList();

        Assert.That(messageBundles.Count, Is.EqualTo(1));
        Assert.That(messageBundles[0].Messages.Count, Is.EqualTo(1));
        Assert.That(messageBundles[0].Messages[0], Is.EqualTo(message));
    }

    [Test]
    public void FirstLinkedResultIsRetained()
    {
        var message = CreateLinkedMessage(RemoteOperationLinkedResultState.First, 1);
        var sut = new PhilipsIntellivueLinkedResultAggregator();
        var messageBundles = sut.TestMessageAndAggregateOrRelease(message).ToList();

        Assert.That(messageBundles.Count, Is.EqualTo(0));
    }

    [Test]
    public void LastLinkedResultIsReleased()
    {
        var message = CreateLinkedMessage(RemoteOperationLinkedResultState.Last, 2);
        var sut = new PhilipsIntellivueLinkedResultAggregator();
        var messageBundles = sut.TestMessageAndAggregateOrRelease(message).ToList();

        Assert.That(messageBundles.Count, Is.EqualTo(1));
        Assert.That(messageBundles[0].Messages.Count, Is.EqualTo(1));
        Assert.That(messageBundles[0].Messages[0], Is.EqualTo(message));
    }

    [Test]
    public void FirstLinkedMessageIsReleasedWhenSecondFirstMessageIsTested()
    {
        var message1 = CreateLinkedMessage(RemoteOperationLinkedResultState.First, 1);
        var message2 = CreateLinkedMessage(RemoteOperationLinkedResultState.First, 1);
        var sut = new PhilipsIntellivueLinkedResultAggregator();
        var messageBundles1 = sut.TestMessageAndAggregateOrRelease(message1).ToList();

        Assert.That(messageBundles1.Count, Is.EqualTo(0));

        var messageBundles2 = sut.TestMessageAndAggregateOrRelease(message2).ToList();

        Assert.That(messageBundles2.Count, Is.EqualTo(1));
        Assert.That(messageBundles2[0].Messages.Count, Is.EqualTo(1));
        Assert.That(messageBundles2[0].Messages[0], Is.EqualTo(message1));
    }

    [Test]
    public void FirstLinkedMessageIsReleasedWhenNonlinkedMessageIsTested()
    {
        var message1 = CreateLinkedMessage(RemoteOperationLinkedResultState.First, 1);
        var message2 = CreateUnlinkedMessage();
        var sut = new PhilipsIntellivueLinkedResultAggregator();
        var messageBundles1 = sut.TestMessageAndAggregateOrRelease(message1).ToList();

        Assert.That(messageBundles1.Count, Is.EqualTo(0));

        var messageBundles2 = sut.TestMessageAndAggregateOrRelease(message2).ToList();

        // Linked message is released
        Assert.That(messageBundles2.Count, Is.EqualTo(2));
        Assert.That(messageBundles2[0].Messages.Count, Is.EqualTo(1));
        Assert.That(messageBundles2[0].Messages[0], Is.EqualTo(message1));

        // Non-linked message is also released
        Assert.That(messageBundles2[1].Messages.Count, Is.EqualTo(1));
        Assert.That(messageBundles2[1].Messages[0], Is.EqualTo(message2));
    }

    [Test]
    public void MessageBundleIsReleasedWhenLastLinkedResultReceived()
    {
        var message1 = CreateLinkedMessage(RemoteOperationLinkedResultState.First, 1);
        var message2 = CreateLinkedMessage(RemoteOperationLinkedResultState.NotFirstNotLast, 2);
        var message3 = CreateLinkedMessage(RemoteOperationLinkedResultState.Last, 3);
        var sut = new PhilipsIntellivueLinkedResultAggregator();

        var messageBundles1 = sut.TestMessageAndAggregateOrRelease(message1).ToList();
        Assert.That(messageBundles1.Count, Is.EqualTo(0));

        var messageBundles2 = sut.TestMessageAndAggregateOrRelease(message2).ToList();
        Assert.That(messageBundles2.Count, Is.EqualTo(0));

        var messageBundles3 = sut.TestMessageAndAggregateOrRelease(message3).ToList();
        Assert.That(messageBundles3.Count, Is.EqualTo(1));
        Assert.That(messageBundles3[0].Messages.Count, Is.EqualTo(3));
        Assert.That(messageBundles3[0].Messages, Is.EqualTo(new[] { message1, message2, message3 }));
    }

    private static DataExportCommandMessage CreateUnlinkedMessage()
    {
        var messageCreator = new CommandMessageCreator();
        return messageCreator.CreatePollReply(
            0,
            1,
            new PollMdiDataRequest(1, new(NomenclaturePartition.NOM_PART_SCADA, (ushort)SCADAType.NOM_RESP_RATE), OIDType.NOM_ATTR_GRP_METRIC_VAL_OBS),
            new RelativeTime(49483832),
            new PatientMonitorDataLogger.PhilipsIntellivue.Models.List<ObservationPoll>([]));
    }

    private static DataExportCommandMessage CreateLinkedMessage(
        RemoteOperationLinkedResultState linkedState,
        byte sequenceNumer)
    {
        var pollMdiDataReply = new PollMdiDataReply(
            1, 
            new RelativeTime(1), 
            AbsoluteTime.Invalid, 
            new(NomenclaturePartition.NOM_PART_SCADA, (ushort)SCADAType.NOM_RESP_RATE), 
            OIDType.NOM_ATTR_GRP_METRIC_VAL_OBS, 
            new PatientMonitorDataLogger.PhilipsIntellivue.Models.List<SingleContextPoll>([]));
        var actionResult = new ActionResultCommand(
            new(OIDType.NOM_MOC_VMS_MDS, new(0, 0)), 
            OIDType.NOM_ACT_POLL_MDIB_DATA,
            0,
            pollMdiDataReply);
        return new DataExportCommandMessage(
            new SessionPresentationHeader(1),
            new RemoteOperationHeader(RemoteOperationType.LinkedResult, 0),
            new RemoteOperationLinkedResult(
                new RemoteOperationLinkedResultId(linkedState, sequenceNumer),
                1,
                DataExportCommandType.ConfirmedAction,
                0,
                actionResult));
    }
}