namespace PatientMonitorDataLogger.BBraun.Models;

[Flags]
public enum PumpAlarm1 : uint
{
    NoAlarm = 1 << 0,
    CalibrationDataCorrupt = 1 << 1,
    BatteryAlarm = 1 << 2,
    TherapyDataCorrupt = 1 << 3,
    TherapyDataAndPumpSettingsCorrupt = 1 << 4,
    BatteryVoltageTooLow = 1 << 5,
    BatteryNotInPump = 1 << 6,
    BatteryFlatAtPowerOn = 1 << 7,
    BatteryCoverOpen = 1 << 8,
    SyringeEmpty = 1 << 9,
    PressureTooHigh = 1 << 10,
    PressureTooHighDriveBlocked = 1 << 11,
    StandbyTimeExpired = 1 << 12,
    VolumeInfused = 1 << 13,
    InfusionTimeExpired = 1 << 14,
    ClawMalfunction = 1 << 15,
    PressureSensorDefect = 1 << 16,
    SyringeHolderOpen = 1 << 17,
    SyrringeIncorrectlyInserted = 1 << 18,
    KvoEnd = 1 << 19,
    PressureAlarmUpstreamSensor = 1 << 20,
    Dummy = 1 << 21,
    DropAlarmSummary = 1 << 22,
    DropAlarmNoDrops = 1 << 23,
    DropAlarmTooFewDrops = 1 << 24,
    DropAlarmTooManyDrops = 1 << 25,
    DropAlarmFreeFlow = 1 << 26,
    DropAlarmDropSensorRequired = 1 << 27,
    AirAlarmSummary = 1 << 28,
    AirAlarmBubbleTooLarge = 1 << 29,
    AirAlarmAirRateExceeded = 1 << 30,
    AirAlarmSensorFault = 1u << 31
}