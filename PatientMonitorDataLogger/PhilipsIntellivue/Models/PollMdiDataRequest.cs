using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PollMdiDataRequest : IActionData
{
    public PollMdiDataRequest(
        ushort pollNumber,
        NomenclatureReference objectType,
        OIDType attributeGroup)
    {
        PollNumber = pollNumber;
        ObjectType = objectType;
        AttributeGroup = attributeGroup;
    }

    public ushort PollNumber { get; }
    public NomenclatureReference ObjectType { get; }
    public OIDType AttributeGroup { get; }

    public virtual byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(PollNumber),
            ..ObjectType.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)AttributeGroup)
        ];
    }

    public static PollMdiDataRequest Read(
        BigEndianBinaryReader binaryReader)
    {
        var pollNumber = binaryReader.ReadUInt16();
        var objectType = NomenclatureReference.Read(binaryReader);
        var attributeGroup = (OIDType)binaryReader.ReadUInt16();
        return new(pollNumber, objectType, attributeGroup);
    }
}