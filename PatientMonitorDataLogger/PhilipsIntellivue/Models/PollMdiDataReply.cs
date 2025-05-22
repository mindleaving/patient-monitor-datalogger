using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PollMdiDataReply : IActionResultData
{
    public PollMdiDataReply() { }

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

    public ushort PollNumber { get; set; }
    public RelativeTime RelativeTimeStamp { get; set; }
    public AbsoluteTime AbsoluteTimeStamp { get; set; }
    public NomenclatureReference ObjectType { get; set; }
    public OIDType AttributeGroup { get; set; }
    public List<SingleContextPoll> PollContexts { get; set; }

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
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var pollNumber = binaryReader.ReadUInt16();
        var relativeTime = RelativeTime.Read(binaryReader);
        var absoluteTime = AbsoluteTime.Read(binaryReader);
        var objectType = NomenclatureReference.Read(binaryReader);
        var attributeGroup = (OIDType)binaryReader.ReadUInt16();
        var pollContexts = List<SingleContextPoll>.Read(binaryReader, x => SingleContextPoll.Read(x, context));
        return new(
            pollNumber,
            relativeTime,
            absoluteTime,
            objectType,
            attributeGroup,
            pollContexts);
    }
}