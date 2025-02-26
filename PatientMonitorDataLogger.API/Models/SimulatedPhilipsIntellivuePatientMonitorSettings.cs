namespace PatientMonitorDataLogger.API.Models;

public class SimulatedPhilipsIntellivuePatientMonitorSettings : PatientMonitorSettings
{
    public override PatientMonitorType Type => PatientMonitorType.SimulatedPhilipsIntellivue;
}