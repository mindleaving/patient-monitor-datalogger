using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Models;

public class BBraunInfusionPumpInfo : IInfusionPumpInfo
{
    public BBraunInfusionPumpInfo() {}
    public BBraunInfusionPumpInfo(
        string bedId)
    {
        BedId = bedId;
    }

    public MedicalDeviceType DeviceType => MedicalDeviceType.InfusionPumps;
    public InfusionPumpType InfusionPumpType => InfusionPumpType.BBraunSpace;

    public string BedId { get; set; }
}