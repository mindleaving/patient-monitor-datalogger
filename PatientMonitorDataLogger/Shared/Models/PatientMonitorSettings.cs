namespace PatientMonitorDataLogger.Shared.Models;

public abstract class PatientMonitorSettings : IPatientMonitorSettings
{
    public MedicalDeviceType DeviceType => MedicalDeviceType.PatientMonitor;
    public abstract PatientMonitorType MonitorType { get; }
}