using Newtonsoft.Json;
using PatientMonitorDataLogger.PhilipsIntellivue.Converters;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

[JsonConverter(typeof(RemoteOperationResultJsonConverter))]
public interface IRemoteOperationResult : ISerializable
{
    ushort InvokeId { get; }
    ushort Length { get; }
}