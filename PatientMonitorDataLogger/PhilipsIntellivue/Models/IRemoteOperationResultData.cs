using Newtonsoft.Json;
using PatientMonitorDataLogger.PhilipsIntellivue.Converters;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

[JsonConverter(typeof(RemoteOperationResultDataJsonConverter))]
public interface IRemoteOperationResultData : ICommandData
{
}