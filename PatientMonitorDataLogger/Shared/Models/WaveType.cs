namespace PatientMonitorDataLogger.Shared.Models;

public enum WaveType
{
    Unknown = 0, // For validation
    EcgDefault,
    EcgI,
    EcgII,
    EcgIII,
    EcgV1,
    EcgV2,
    EcgV3,
    EcgV4,
    EcgV5,
    EcgV6,
    Pleth,
    Pleth2,
    ArterialBloodPressure,
    CO2,
    Respiration,
}