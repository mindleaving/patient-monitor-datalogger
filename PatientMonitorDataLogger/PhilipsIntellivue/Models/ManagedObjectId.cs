using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ManagedObjectId : ISerializable
{
    public ManagedObjectId(
        OIDType objectClass,
        GlobalHandle objectInstance)
    {
        ObjectClass = objectClass;
        ObjectInstance = objectInstance;
    }

    public OIDType ObjectClass { get; }
    public GlobalHandle ObjectInstance { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)ObjectClass),
            ..ObjectInstance.Serialize()
        ];
    }

    public static ManagedObjectId Read(
        BigEndianBinaryReader binaryReader)
    {
        var objectClass = (OIDType)binaryReader.ReadUInt16();
        var objectInstance = GlobalHandle.Read(binaryReader);
        return new(objectClass, objectInstance);
    }
}