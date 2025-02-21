namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

[Flags]
public enum AlertFlags : ushort
{
    BEDSIDE_AUDIBLE = 0x4000,
    CENTRAL_AUDIBLE = 0x2000,
    VISUAL_LATCHING = 0x1000,
    AUDIBLE_LATCHING = 0x0800,
    SHORT_YELLOW_EXTENSION = 0x0400,
    DERIVED = 0x0200,
}