namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public interface IRemoteOperation : IRemoteOperationResult
{
    DataExportCommandType CommandType { get; }
}