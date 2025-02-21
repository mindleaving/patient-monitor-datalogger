using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ExtendedPollMdiDataReply : IActionResultData
{
    public ExtendedPollMdiDataReply(
        ushort pollNumber,
        ushort sequenceNumber,
        RelativeTime relativeTimeStamp,
        AbsoluteTime absoluteTimeStamp,
        NomenclatureReference objectType,
        OIDType attributeGroup,
        List<SingleContextPoll> pollContexts)
    {
        PollNumber = pollNumber;
        SequenceNumber = sequenceNumber;
        RelativeTimeStamp = relativeTimeStamp;
        AbsoluteTimeStamp = absoluteTimeStamp;
        ObjectType = objectType;
        AttributeGroup = attributeGroup;
        PollContexts = pollContexts;
    }

    public ushort PollNumber { get; }
    public ushort SequenceNumber { get; }
    public RelativeTime RelativeTimeStamp { get; }
    public AbsoluteTime AbsoluteTimeStamp { get; }
    public NomenclatureReference ObjectType { get; }
    public OIDType AttributeGroup { get; }
    public List<SingleContextPoll> PollContexts { get; }

    public byte[] Serialize()
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

    public static ExtendedPollMdiDataReply Read(
        BigEndianBinaryReader binaryReader)
    {
        var pollNumber = binaryReader.ReadUInt16();
        var sequenceNumber = binaryReader.ReadUInt16();
        var relativeTimeStamp = RelativeTime.Read(binaryReader);
        var absoluteTimeStamp = AbsoluteTime.Read(binaryReader);
        var objectType = NomenclatureReference.Read(binaryReader);
        var attributeGroup = (OIDType)binaryReader.ReadUInt16();
        var pollContexts = List<SingleContextPoll>.Read(binaryReader, SingleContextPoll.Read);
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