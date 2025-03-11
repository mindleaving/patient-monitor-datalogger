namespace PatientMonitorDataLogger.BBraun.Models;

[Flags]
public enum PreAlarmReason : ushort
{
    NoAlarm = 0,
    SyringeEmpty = 1 << 1,
    VolumeAlmostDelivered = 1 << 2,
    TimeNearEnd = 1 << 3,
    BatteryAlmostFlat = 1 << 4,
    KvoOperation = 1 << 5,
    UnauthorizedManipulation = 1 << 6,
    IncompatibleCanBusDevice = 1 << 7,
    PiggybackSecondaryVolumeInfused =  1 << 8,
    SgcPreAlarm = 1 << 9,
    IdpgOcclusion = 1 << 10,
    IdpgPressureLeapHigh = 1 << 11,
    IdpgPressureLeapLow = 1 << 12,
    SyringePreAlarmInTomMaster = 1 << 13,
    SlaveFailureInTomMaster = 1 << 14,
    BatteryDefective = 1 << 15
}