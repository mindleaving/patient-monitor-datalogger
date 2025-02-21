using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ExtendedPollMdiDataRequest : PollMdiDataRequest
{
    public ExtendedPollMdiDataRequest(
        ushort pollNumber,
        NomenclatureReference objectType,
        OIDType attributeGroup,
        List<AttributeValueAssertion> attributes)
        : base(pollNumber, objectType, attributeGroup)
    {
        Attributes = attributes;
    }

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

    public override byte[] Serialize()
    {
        return
        [
            ..base.Serialize(),
            ..Attributes.Serialize()
        ];
    }

    public new static ExtendedPollMdiDataRequest Read(
        BigEndianBinaryReader binaryReader)
    {
        var baseObject = PollMdiDataRequest.Read(binaryReader);
        var attributes = List<AttributeValueAssertion>.Read(binaryReader, AttributeValueAssertion.Read);
        return new(
            baseObject.PollNumber,
            baseObject.ObjectType,
            baseObject.AttributeGroup,
            attributes);
    }
}