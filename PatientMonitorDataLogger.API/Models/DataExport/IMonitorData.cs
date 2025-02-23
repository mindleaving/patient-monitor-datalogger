namespace PatientMonitorDataLogger.API.Models.DataExport;

public interface IMonitorData
{
    MonitorDataType Type { get; }
    Guid LogSessionId { get; }
}