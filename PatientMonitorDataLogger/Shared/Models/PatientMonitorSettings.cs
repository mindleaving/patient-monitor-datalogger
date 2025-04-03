namespace PatientMonitorDataLogger.Shared.Models;

public abstract class PatientMonitorSettings : IMedicalDeviceSettings
{
    public MedicalDeviceType DeviceType => MedicalDeviceType.PatientMonitor;
    public abstract PatientMonitorType MonitorType { get; }
}