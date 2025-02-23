using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SingleContextPoll : ISerializable
{
    public SingleContextPoll(
        ushort contextId,
        List<ObservationPoll> observations)
    {
        ContextId = contextId;
        Observations = observations;
    }

    public ushort ContextId { get; }
    public List<ObservationPoll> Observations { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(ContextId),
            ..Observations.Serialize()
        ];
    }

    public static SingleContextPoll Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var contextId = binaryReader.ReadUInt16();
        var pollInfo = List<ObservationPoll>.Read(binaryReader, x => ObservationPoll.Read(x, context));
        return new(contextId, pollInfo);
    }
}