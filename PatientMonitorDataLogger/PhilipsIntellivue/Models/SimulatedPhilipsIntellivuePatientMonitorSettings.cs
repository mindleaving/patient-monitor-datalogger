using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SimulatedPhilipsIntellivuePatientMonitorSettings : PatientMonitorSettings
{
    public override PatientMonitorType MonitorType => PatientMonitorType.SimulatedPhilipsIntellivue;
}