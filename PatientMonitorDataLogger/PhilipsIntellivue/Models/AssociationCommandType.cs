namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public enum AssociationCommandType : byte
{
    RequestAssociation = 0x0D,
    AssociationAccepted = 0x0E,
    Refuse = 0xC,
    RequestRelease = 0x09,
    Released = 0xA,
    Abort = 0x19
}