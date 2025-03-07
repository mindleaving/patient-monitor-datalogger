namespace PatientMonitorDataLogger.Shared.Models;

public class InfusionPumpDataSettings : IMedicalDeviceDataSettings
{
    public MedicalDeviceType DeviceType => MedicalDeviceType.InfusionPumps;
}