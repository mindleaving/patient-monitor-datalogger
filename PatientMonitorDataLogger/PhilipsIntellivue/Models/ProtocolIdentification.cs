namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public enum ProtocolIdentification : ushort
{
    NOM_POLL_PROFILE_SUPPORT = 1, // id for polling profile
    NOM_MDIB_OBJ_SUPPORT = 258, // supported objects for the active profile
    NOM_ATTR_POLL_PROFILE_EXT = 61441, // id for poll profile extensions opt. package
}