namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public enum PatientState : ushort
{
    EMPTY = 0,
    PRE_ADMITTED = 1,
    ADMITTED = 2,
    DISCHARGED = 8,
}