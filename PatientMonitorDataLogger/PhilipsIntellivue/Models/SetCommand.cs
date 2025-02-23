using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SetCommand : IRemoteOperationInvokeData
{
    public SetCommand(
        ManagedObjectId managedObject,
        List<AttributeModification> modifications)
    {
        ManagedObject = managedObject;
        Modifications = modifications;
    }

    public ManagedObjectId ManagedObject { get; }
    public uint Scope { get; } = 0;
    public List<AttributeModification> Modifications { get; }

    public byte[] Serialize()
    {
        return
        [
            ..ManagedObject.Serialize(),
            ..BigEndianBitConverter.GetBytes(Scope),
            ..Modifications.Serialize()
        ];
    }

    public static SetCommand Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var scope = binaryReader.ReadUInt32();
        var modificationList = List<AttributeModification>.Read(binaryReader, x => AttributeModification.Read(x, context));
        return new(managedObject, modificationList);
    }
}