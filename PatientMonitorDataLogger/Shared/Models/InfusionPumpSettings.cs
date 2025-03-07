namespace PatientMonitorDataLogger.Shared.Models;

public abstract class InfusionPumpSettings : IInfusionPumpSettings
{
    public MedicalDeviceType DeviceType => MedicalDeviceType.InfusionPumps;
    public abstract InfusionPumpType InfusionPumpType { get; }
}