namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public enum MeasurementState : ushort
{
    VALID = 0,
    INVALID = 0x8000,
    QUESTIONABLE = 0x4000,
    UNAVAILABLE = 0x2000,
    CALIBRATION_ONGOING = 0x1000,
    TEST_DATA = 0x0800,
    DEMO_DATA = 0x0400,
    VALIDATED_DATA = 0x0080,
    EARLY_INDICATION = 0x0040,
    MSMT_ONGOING = 0x0020,
    MSMT_STATE_IN_ALARM = 0x0002,
    MSMT_STATE_AL_INHIBITED = 0x0001,
}