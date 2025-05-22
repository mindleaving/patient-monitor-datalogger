using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Converters;

public class RemoteOperationResultDataJsonConverter : ReadonlyJsonConverter<IRemoteOperationResultData>
{
    protected override IRemoteOperationResultData? SelectModel(
        JObject jObject)
    {
        if (jObject.ContainsKey(nameof(ActionResultCommand.ActionType)))
            return new ActionResultCommand();

        if (jObject.ContainsKey(nameof(EventReportResultCommand.EventType)))
            return new EventReportResultCommand();

        // Missing: How to distinguish SetResultCommand and GetResultCommand. GetResultCommand more likely...
        return new GetResultCommand();

    }
}