namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public enum AlertState : ushort
{
    AL_INHIBITED = 0x8000,
    AL_SUSPENDED = 0x4000,
    AL_LATCHED = 0x2000,
    AL_SILENCED_RESET = 0x1000,
    AL_DEV_IN_TEST_MODE = 0x0400,
    AL_DEV_IN_STANDBY = 0x0200,
    AL_DEV_IN_DEMO_MODE = 0x0100,
    AL_NEW_ALERT = 0x0008,
}