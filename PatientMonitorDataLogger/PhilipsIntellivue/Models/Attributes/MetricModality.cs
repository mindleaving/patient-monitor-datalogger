namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public enum MetricModality : ushort
{
    METRIC_MODALITY_MANUAL = 0x4000,
    METRIC_MODALITY_APERIODIC = 0x2000,
    METRIC_MODALITY_VERIFIED = 0x1000
}