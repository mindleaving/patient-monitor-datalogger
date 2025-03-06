using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public interface IRemoteOperationResult : ISerializable
{
    ushort InvokeId { get; }
    ushort Length { get; }
}