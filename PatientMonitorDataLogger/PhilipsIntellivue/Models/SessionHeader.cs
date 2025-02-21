namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SessionHeader
{
    public SessionHeader(
        AssociationCommandType type,
        LengthIndicator length)
    {
        Type = type;
        Length = length;
    }

    public AssociationCommandType Type { get; }
    public LengthIndicator Length { get; }

    public byte[] Serialize()
    {
        return [(byte)Type, ..Length.Serialize()];
    }

    public static SessionHeader Read(
        Stream stream)
    {
        var type = (AssociationCommandType)stream.ReadByte();
        var length = LengthIndicator.Read(stream);
        return new(type, length);
    }
}