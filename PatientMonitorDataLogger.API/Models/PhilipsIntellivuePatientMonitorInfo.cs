using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Models;

public class PhilipsIntellivuePatientMonitorInfo : IPatientMonitorInfo
{
    public MedicalDeviceType DeviceType => MedicalDeviceType.PatientMonitor;
    public PatientMonitorType MonitorType => PatientMonitorType.PhilipsIntellivue;
    public string Name { get; set; } = "(no name)";
}