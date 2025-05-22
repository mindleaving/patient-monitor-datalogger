namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class DataExportCommandMessage : ICommandMessage
{
    public DataExportCommandMessage() { }
    public DataExportCommandMessage(
        SessionPresentationHeader sessionPresentationHeader,
        RemoteOperationHeader remoteOperationHeader,
        IRemoteOperationResult remoteOperationData)
    {
        SessionPresentationHeader = sessionPresentationHeader;
        RemoteOperationHeader = remoteOperationHeader;
        RemoteOperationData = remoteOperationData;
    }

    public SessionPresentationHeader SessionPresentationHeader { get; set; }
    public RemoteOperationHeader RemoteOperationHeader { get; set; }
    public IRemoteOperationResult RemoteOperationData { get; set; }

    public CommandMessageType MessageType => CommandMessageType.DataExport;

    public byte[] Serialize()
    {
        return
        [
            ..SessionPresentationHeader.Serialize(),
            ..RemoteOperationHeader.Serialize(),
            ..RemoteOperationData.Serialize()
        ];
    }

    public override string ToString()
    {
        string operationDescription;
        switch (RemoteOperationData)
        {
            case RemoteOperationError remoteOperationError:
            {
                operationDescription = $"Error {remoteOperationError.ErrorValue}";
                break;
            }
            case RemoteOperationInvoke remoteOperationInvoke:
            {
                var invokeDescription = remoteOperationInvoke.Data switch
                {
                    ActionCommand actionCommand => $"Action {actionCommand.ActionType}",
                    EventReportCommand eventReportCommand => $"Event report {eventReportCommand.EventType}",
                    GetCommand getCommand => $"Get {string.Join(", ", getCommand.AttributeIdList.Values)}",
                    SetCommand setCommand => $"Modifying {setCommand.Modifications.Values.Count} attributes",
                    _ => throw new ArgumentOutOfRangeException()
                };
                operationDescription = $"Invoke {remoteOperationInvoke.CommandType}, {invokeDescription}";
                break;
            }
            case RemoteOperationLinkedResult remoteOperationLinkedResult:
            {
                var resultDescription = FormatRemoteOperationResult(remoteOperationLinkedResult.Data);
                operationDescription = $"Linked result of type {remoteOperationLinkedResult.Data.GetType().Name} for {resultDescription}";
                break;
            }
            case RemoteOperationResult remoteOperationResult:
            {
                var resultDescription = FormatRemoteOperationResult(remoteOperationResult.Data);
                operationDescription = $"Result of type {remoteOperationResult.Data.GetType().Name} for {resultDescription}";
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(RemoteOperationData));
        }
        return $"Data Export {RemoteOperationHeader.Type}, {operationDescription}";
    }

    private static string FormatRemoteOperationResult(
        IRemoteOperationResultData resultData)
    {
        var resultDescription = resultData switch
        {
            ActionResultCommand actionResultCommand => $"Action {actionResultCommand.ActionType}",
            EventReportResultCommand eventReportResultCommand => $"Event report result for event {eventReportResultCommand.EventType}",
            GetResultCommand getResultCommand => $"Get result for {getResultCommand.AttributeList.Count} attributes",
            SetResultCommand setResultCommand => $"Set result for {setResultCommand.AttributeList.Count} attributes",
            _ => throw new ArgumentOutOfRangeException()
        };
        return resultDescription;
    }
}