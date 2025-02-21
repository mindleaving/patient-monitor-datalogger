using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class MdsCreateInfo : IEventReportData
{
    public MdsCreateInfo(
        ManagedObjectId managedObject,
        List<AttributeValueAssertion> attributeList)
    {
        ManagedObject = managedObject;
        AttributeList = attributeList;
    }

    public ManagedObjectId ManagedObject { get; }
    public List<AttributeValueAssertion> AttributeList { get; }

    public static MdsCreateInfo Read(
        BigEndianBinaryReader binaryReader)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var attributeList = List<AttributeValueAssertion>.Read(binaryReader, AttributeValueAssertion.Read);
        return new(managedObject, attributeList);
    }

    public byte[] Serialize()
    {
        return
        [
            ..ManagedObject.Serialize(),
            ..AttributeList.Serialize()
        ];
    }
}