namespace PatientMonitorDataLogger.Shared.Models;

public interface IInfusionPumpInfo : IMedicalDeviceInfo
{
    InfusionPumpType InfusionPumpType { get; }
}