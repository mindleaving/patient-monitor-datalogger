using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Models;

public interface IPatientMonitorInfo : IMedicalDeviceInfo
{
    PatientMonitorType MonitorType { get; }
}