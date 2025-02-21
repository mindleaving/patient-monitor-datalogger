using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class NoSuchObjectClassError : IRemoteOperationErrorData
{
    public NoSuchObjectClassError(
        OIDType objectClass)
    {
        ObjectClass = objectClass;
    }

    public OIDType ObjectClass { get; }

    public static NoSuchObjectClassError Read(
        BigEndianBinaryReader binaryReader)
    {
        var objectClass = (OIDType)binaryReader.ReadUInt16();
        return new(objectClass);
    }

    public byte[] Serialize()
    {
        return BigEndianBitConverter.GetBytes((ushort)ObjectClass);
    }
}