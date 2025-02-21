using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PollMdiDataReply : IActionResultData
{
    public PollMdiDataReply(
        ushort pollNumber,
        RelativeTime relativeTimeStamp,
        AbsoluteTime absoluteTimeStamp,
        NomenclatureReference objectType,
        OIDType attributeGroup,
        List<SingleContextPoll> pollContexts)
    {
        PollNumber = pollNumber;
        RelativeTimeStamp = relativeTimeStamp;
        AbsoluteTimeStamp = absoluteTimeStamp;
        ObjectType = objectType;
        AttributeGroup = attributeGroup;
        PollContexts = pollContexts;
    }

    public ushort PollNumber { get; }
    public RelativeTime RelativeTimeStamp { get; }
    public AbsoluteTime AbsoluteTimeStamp { get; }
    public NomenclatureReference ObjectType { get; }
    public OIDType AttributeGroup { get; }
    public List<SingleContextPoll> PollContexts { get; }

    public virtual byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(PollNumber),
            ..RelativeTimeStamp.Serialize(),
            ..AbsoluteTimeStamp.Serialize(),
            ..ObjectType.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)AttributeGroup),
            ..PollContexts.Serialize()
        ];
    }

    public static PollMdiDataReply Read(
        BigEndianBinaryReader binaryReader)
    {
        var pollNumber = binaryReader.ReadUInt16();
        var relativeTime = RelativeTime.Read(binaryReader);
        var absoluteTime = AbsoluteTime.Read(binaryReader);
        var objectType = NomenclatureReference.Read(binaryReader);
        var attributeGroup = (OIDType)binaryReader.ReadUInt16();
        var pollContexts = List<SingleContextPoll>.Read(binaryReader, SingleContextPoll.Read);
        return new(
            pollNumber,
            relativeTime,
            absoluteTime,
            objectType,
            attributeGroup,
            pollContexts);
    }
}