namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

[Flags]
public enum PollProfileOptions : uint
{
    None = 0,
    P_OPT_DYN_CREATE_OBJECTS = 0x40000000,
    P_OPT_DYN_DELETE_OBJECTS = 0x20000000
}