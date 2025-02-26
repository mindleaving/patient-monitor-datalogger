using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ExtendedPollMdiDataRequest : IActionData
{
    public ExtendedPollMdiDataRequest(
        ushort pollNumber,
        NomenclatureReference objectType,
        OIDType attributeGroup,
        List<AttributeValueAssertion> attributes)
    {
        PollNumber = pollNumber;
        ObjectType = objectType;
        AttributeGroup = attributeGroup;
        Attributes = attributes;
    }

    public ushort PollNumber { get; }
    public NomenclatureReference ObjectType { get; }
    public OIDType AttributeGroup { get; }
    public List<AttributeValueAssertion> Attributes { get; }

    public void SetTimePeriodicDataPoll(
        TimeSpan period)
    {
        var relativeTime = new RelativeTime(period);
        Attributes.Values.Add(new((ushort)OIDType.NOM_ATTR_TIME_PD_POLL, relativeTime));
    }

    public void SetNumberOfPrioritizedObjects(
        ushort count)
    {
        Attributes.Values.Add(new((ushort)OIDType.NOM_ATTR_POLL_OBJ_PRIO_NUM, new UshortAttributeValue(count)));
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(PollNumber),
            ..ObjectType.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)AttributeGroup),
            ..Attributes.Serialize()
        ];
    }

    public static ExtendedPollMdiDataRequest Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var pollNumber = binaryReader.ReadUInt16();
        var objectType = NomenclatureReference.Read(binaryReader);
        var attributeGroup = (OIDType)binaryReader.ReadUInt16();
        var attributes = List<AttributeValueAssertion>.Read(binaryReader, x => AttributeValueAssertion.Read(x, context));
        return new(
            pollNumber,
            objectType,
            attributeGroup,
            attributes);
    }
}