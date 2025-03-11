using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SimulatedPhilipsIntellivueSettings : PatientMonitorSettings
{
    public override PatientMonitorType MonitorType => PatientMonitorType.SimulatedPhilipsIntellivue;
}