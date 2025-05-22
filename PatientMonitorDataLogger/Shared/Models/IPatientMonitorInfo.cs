namespace PatientMonitorDataLogger.Shared.Models;

public interface IPatientMonitorInfo : IMedicalDeviceInfo
{
    PatientMonitorType MonitorType { get; }
}