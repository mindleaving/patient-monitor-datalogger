namespace PatientMonitorDataLogger.DataExport.Models;

public interface IMonitorData
{
    MonitorDataType Type { get; }
    Guid LogSessionId { get; }
}