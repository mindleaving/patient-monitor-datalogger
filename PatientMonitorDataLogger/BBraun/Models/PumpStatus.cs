namespace PatientMonitorDataLogger.BBraun.Models;

[Flags]
public enum PumpStatus : uint
{
    PumpSwitchedOn = 1 << 0,
    MainsPowerOperation = 1 << 1,
    StandbyActive = 1 << 2,
    StartupMenuActive = 1 << 3,
    PrimeActive = 1 << 4,
    DriveInParkPosition = 1 << 5,
    PumpReadyToStart = 1 << 6,
    PumpInfusing = 1 << 7,
    TherapyStarted = 1 << 8,
    ReadyToStartBolus = 1 << 9,
    ManualBolusActive = 1 << 10,
    VolumeBolusIsActive = 1 << 11,
    KvoFunctionReleased = 1 << 12,
    KvoActive = 1 << 13,
    DataLockActive = 1 << 14,
    PumpInDoseMode = 1 << 15,
    DoseBolusWithBodyWeight = 1 << 16,
    PatientStoredInPump = 1 << 17,
    AlarmSilenced = 1 << 19,
    PreAlarmSilenced = 1 << 20,
    SpaceControlConnected = 1 << 21,
    AirSensorSwitchedOff = 1 << 22,
    PrimaryActive = 1 << 23,
    ParameterEditorActive = 1 << 24
}