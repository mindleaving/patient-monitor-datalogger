namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public enum MetricCategory : ushort
{
    MCAT_UNSPEC = 0,
    AUTO_MEASUREMENT = 1,
    MANUAL_MEASUREMENT = 2,
    AUTO_SETTING = 3,
    MANUAL_SETTING = 4,
    AUTO_CALCULATION = 5,
    MANUAL_CALCULATION = 6,
    MULTI_DYNAMIC_CAPABILITIES = 50,
    AUTO_ADJUST_PAT_TEMP = 128,
    MANUAL_ADJUST_PAT_TEMP = 129,
    AUTO_ALARM_LIMIT_SETTING = 130,
}