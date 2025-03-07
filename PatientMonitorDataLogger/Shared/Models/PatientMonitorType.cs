namespace PatientMonitorDataLogger.Shared.Models;

public enum PatientMonitorType
{
    Unknown = 0, // For validation
    PhilipsIntellivue = 1,
    GEDash = 2,
    SimulatedPhilipsIntellivue = 91,
}