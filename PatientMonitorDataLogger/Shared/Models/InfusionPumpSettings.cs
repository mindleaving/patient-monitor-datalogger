namespace PatientMonitorDataLogger.Shared.Models;

public abstract class InfusionPumpSettings : IMedicalDeviceSettings
{
    public MedicalDeviceType DeviceType => MedicalDeviceType.InfusionPumps;
    public abstract InfusionPumpType InfusionPumpType { get; }
}