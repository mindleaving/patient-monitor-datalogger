namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public enum MeasureMode : ushort
{
    CO2_SIDESTREAM = 0x0400,
    ECG_PACED = 0x0200,
    ECG_NONPACED = 0x0100,
    ECG_DIAG = 0x0080,
    ECG_MONITOR = 0x0040,
    ECG_FILTER = 0x0020,
    ECG_MODE_EASI = 0x0008,
    ECG_LEAD_PRIMARY = 0x0004
}