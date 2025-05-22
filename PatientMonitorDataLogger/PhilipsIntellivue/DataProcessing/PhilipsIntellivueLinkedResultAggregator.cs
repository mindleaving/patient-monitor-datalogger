using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.DataProcessing;

public class PhilipsIntellivueLinkedResultAggregator
{
    private readonly System.Collections.Generic.List<ICommandMessage> retainedLinkedResultMessages = new();

    public bool IsLinkedResult(
        ICommandMessage message)
    {
        if (message is not DataExportCommandMessage dataExportCommandMessage)
            return false;
        return dataExportCommandMessage.RemoteOperationHeader.Type == RemoteOperationType.LinkedResult;
    }

    public bool IsRetainingMessages => retainedLinkedResultMessages.Any();

    public IEnumerable<LinkedCommandMessageBundle> TestMessageAndAggregateOrRelease(
        ICommandMessage message)
    {
        if (!IsLinkedResult(message))
        {
            if (retainedLinkedResultMessages.Any())
                yield return ReleaseRetainedLinkedMessages();
            yield return new LinkedCommandMessageBundle([message]);
            yield break;
        }

        if (IsFirstResult(message))
        {
            // Release retained messages, if a new first message is received.
            // Usually there should not be any, because a "Last" result was received before,
            // but in case messages were missed, release the unfinished bundle
            if (retainedLinkedResultMessages.Any())
                yield return ReleaseRetainedLinkedMessages();
        }
        retainedLinkedResultMessages.Add(message);
        if (IsLastResult(message))
            yield return ReleaseRetainedLinkedMessages();
    }

    private bool IsFirstResult(
        ICommandMessage message)
    {
        if (message is not DataExportCommandMessage dataExportCommandMessage)
            return false;
        if (dataExportCommandMessage.RemoteOperationHeader.Type != RemoteOperationType.LinkedResult)
            return false;
        if (dataExportCommandMessage.RemoteOperationData is not RemoteOperationLinkedResult linkedResult)
            return false;
        return linkedResult.LinkedId.State == RemoteOperationLinkedResultState.First;
    }

    private bool IsLastResult(
        ICommandMessage message)
    {
        if (message is not DataExportCommandMessage dataExportCommandMessage)
            return false;
        if (dataExportCommandMessage.RemoteOperationHeader.Type != RemoteOperationType.LinkedResult)
            return false;
        if (dataExportCommandMessage.RemoteOperationData is not RemoteOperationLinkedResult linkedResult)
            return false;
        return linkedResult.LinkedId.State == RemoteOperationLinkedResultState.Last;
    }

    public LinkedCommandMessageBundle ReleaseRetainedLinkedMessages()
    {
        if (retainedLinkedResultMessages.Count == 0)
            throw new InvalidOperationException("Cannot release linked messages. No retained messages.");
        var bundle = new LinkedCommandMessageBundle([..retainedLinkedResultMessages]);
        retainedLinkedResultMessages.Clear();
        return bundle;
    }
}