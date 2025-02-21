namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public enum StartupMode : uint
{
    HotStart = 0x80000000,
    WarmStart = 0x40000000,
    ColdStart = 0x20000000
}