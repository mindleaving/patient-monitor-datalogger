using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ExtendedPollMdiDataReply : PollMdiDataReply
{
    public ExtendedPollMdiDataReply(
        ushort pollNumber,
        ushort sequenceNumber,
        RelativeTime relativeTimeStamp,
        AbsoluteTime absoluteTimeStamp,
        NomenclatureReference objectType,
        OIDType attributeGroup,
        List<SingleContextPoll> pollContexts)
        : base(pollNumber, relativeTimeStamp, absoluteTimeStamp, objectType, attributeGroup, pollContexts)
    {
        SequenceNumber = sequenceNumber;
    }

    public ushort SequenceNumber { get; }

    public override byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(PollNumber),
            ..BigEndianBitConverter.GetBytes(SequenceNumber),
            ..RelativeTimeStamp.Serialize(),
            ..AbsoluteTimeStamp.Serialize(),
            ..ObjectType.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)AttributeGroup),
            ..PollContexts.Serialize()
        ];
    }

    public new static ExtendedPollMdiDataReply Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var pollNumber = binaryReader.ReadUInt16();
        var sequenceNumber = binaryReader.ReadUInt16();
        var relativeTimeStamp = RelativeTime.Read(binaryReader);
        var absoluteTimeStamp = AbsoluteTime.Read(binaryReader);
        var objectType = NomenclatureReference.Read(binaryReader);
        var attributeGroup = (OIDType)binaryReader.ReadUInt16();
        var pollContexts = List<SingleContextPoll>.Read(binaryReader, x => SingleContextPoll.Read(x, context));
        return new(
            pollNumber,
            sequenceNumber,
            relativeTimeStamp,
            absoluteTimeStamp,
            objectType,
            attributeGroup,
            pollContexts);
    }
}