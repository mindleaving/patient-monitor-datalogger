namespace PatientMonitorDataLogger.BBraun.Models;

[Flags]
public enum PumpAlarm2 : uint
{
    Dummy = 1 << 0,
    UnauthorizedManipulation = 1 << 1,
    TimeLimitReached = 1 << 2,
    SgcEndAlarm = 1 << 3,
    TakeOverModeSyringeEnd = 1 << 4,
    TakeOverFailed = 1 << 5,
    TakeOverModeSlave = 1 << 6,
    DangerOfFreeFlow = 1 << 7,
    InfusomatOcclusionTestError = 1 << 29,
    InfusomatDoorOpenedDuringInfusion = 1 << 30,
    PerfusorSyringeNotInsertedSecurely = 1u << 31
}