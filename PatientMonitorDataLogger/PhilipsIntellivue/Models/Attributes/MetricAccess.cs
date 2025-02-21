namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public enum MetricAccess : ushort
{
    AVAIL_INTERMITTEND = 0x8000,
    UPD_PERIODIC = 0x4000,
    UPD_EPISODIC = 0x2000,
    MSMT_NONCONTINUOUS = 0x1000,
}