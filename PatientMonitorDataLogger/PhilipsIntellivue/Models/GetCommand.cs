using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class GetCommand : IRemoteOperationInvokeData
{
    public GetCommand(
        ManagedObjectId managedObject,
        AttributeIdList attributeIdList)
    {
        ManagedObject = managedObject;
        AttributeIdList = attributeIdList;
    }

    public ManagedObjectId ManagedObject { get; }
    public uint Scope { get; } = 0;
    public AttributeIdList AttributeIdList { get; }

    public byte[] Serialize()
    {
        return
        [
            ..ManagedObject.Serialize(),
            ..BigEndianBitConverter.GetBytes(Scope),
            ..AttributeIdList.Serialize()
        ];
    }

    public static GetCommand Read(
        BigEndianBinaryReader binaryReader)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var scope = binaryReader.ReadUInt32();
        var attributeIds = Models.AttributeIdList.Read(binaryReader);
        return new(managedObject, attributeIds);
    }
}