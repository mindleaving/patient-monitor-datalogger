namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public enum AlertType : ushort
{
    NO_ALERT = 0,
    LOW_PRI_T_AL = 1,
    MED_PRI_T_AL = 2,
    HI_PRI_T_AL = 4,
    LOW_PRI_P_AL = 256,
    MED_PRI_P_AL = 512,
    HI_PRI_P_AL = 1024,
}