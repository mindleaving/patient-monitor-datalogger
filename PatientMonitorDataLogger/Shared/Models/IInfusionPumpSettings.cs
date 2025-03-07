namespace PatientMonitorDataLogger.Shared.Models;

public interface IInfusionPumpSettings : IMedicalDeviceSettings
{
    InfusionPumpType InfusionPumpType { get; }
}