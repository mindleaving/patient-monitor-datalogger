namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public enum MonitorConnectionChangeEventType
{
    Unknown = 0, // For validation
    Connected = 1,
    Aborted = 2,
    Disconnected = 3,
    Faulted = 4
}