using Newtonsoft.Json;
using PatientMonitorDataLogger.PhilipsIntellivue.Converters;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

[JsonConverter(typeof(RemoteOperationErrorDataJsonConverter))]
public interface IRemoteOperationErrorData : ISerializable
{
}