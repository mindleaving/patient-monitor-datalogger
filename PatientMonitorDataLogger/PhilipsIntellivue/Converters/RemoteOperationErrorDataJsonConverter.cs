using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Converters;

public class RemoteOperationErrorDataJsonConverter : ReadonlyJsonConverter<IRemoteOperationErrorData>
{
    protected override IRemoteOperationErrorData? SelectModel(
        JObject jObject)
    {
        return null;
    }
}