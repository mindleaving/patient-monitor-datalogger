using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Models.DataExport;

public interface IMonitorData
{
    MonitorDataType Type { get; }
}