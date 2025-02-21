namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public enum SampleArrayFlags : ushort
{
    SMOOTH_CURVE = 0x8000,
    DELAYED_CURVE = 0x4000,
    STATIC_SCALE = 0x2000,
    SA_EXT_VAL_RANGE = 0x1000,
}