using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class GetResultCommand : IRemoteOperationResultData
{
    public GetResultCommand(
        ManagedObjectId managedObject,
        List<AttributeValueAssertion> attributeList)
    {
        ManagedObject = managedObject;
        AttributeList = attributeList;
    }

    public ManagedObjectId ManagedObject { get; }
    public List<AttributeValueAssertion> AttributeList { get; }

    public byte[] Serialize()
    {
        return
        [
            ..ManagedObject.Serialize(),
            ..AttributeList.Serialize()
        ];
    }

    public static GetResultCommand Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var attributes = List<AttributeValueAssertion>.Read(binaryReader, x => AttributeValueAssertion.Read(x, context));
        return new(managedObject, attributes);
    }
}