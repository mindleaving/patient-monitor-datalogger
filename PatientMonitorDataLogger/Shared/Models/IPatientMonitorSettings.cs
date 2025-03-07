namespace PatientMonitorDataLogger.Shared.Models;

public interface IPatientMonitorSettings : IMedicalDeviceSettings
{
    PatientMonitorType MonitorType { get; }
}