namespace PatientMonitorDataLogger.API.Models;

public class PhilipsIntellivuePatientMonitorInfo : IPatientMonitorInfo
{
    public PatientMonitorType Type => PatientMonitorType.PhilipsIntellivue;
    public string Name { get; set; } = "(no name)";
}