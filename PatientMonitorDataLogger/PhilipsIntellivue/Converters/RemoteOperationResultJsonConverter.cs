using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Converters;

public class RemoteOperationResultJsonConverter : ReadonlyJsonConverter<IRemoteOperationResult>
{
    protected override IRemoteOperationResult? SelectModel(
        JObject jObject)
    {
        if (jObject.ContainsKey(nameof(RemoteOperationError.ErrorValue)))
            return new RemoteOperationError();

        if (jObject.ContainsKey(nameof(RemoteOperationLinkedResult.LinkedId)))
            return new RemoteOperationLinkedResult();

        // Missing: Way to distinguish RemoteOperationResult and RemoteOperationInvoke (although RemoteOperationInvoke will usually not be a response in messages.json)
        return new RemoteOperationResult();
    }
}