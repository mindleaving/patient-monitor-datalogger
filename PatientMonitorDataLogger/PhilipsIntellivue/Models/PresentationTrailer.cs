using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PresentationTrailer
{
    public static readonly PresentationTrailer AssociationRequest = new(Enumerable.Repeat<byte>(0x00, 16).ToArray());
    public static readonly PresentationTrailer AssociationResponse = new(Enumerable.Repeat<byte>(0x00, 16).ToArray());
    public static readonly PresentationTrailer ReleaseRequest = new(Enumerable.Repeat<byte>(0x00, 4).ToArray());
    public static readonly PresentationTrailer ReleaseResponse = new(Enumerable.Repeat<byte>(0x00, 4).ToArray());
    public static readonly PresentationTrailer AssociationAbort = new(Enumerable.Repeat<byte>(0x00, 4).ToArray());
    public static readonly PresentationTrailer AssociationRefuse = new([]);

    public PresentationTrailer(
        byte[] payload)
    {
        Payload = payload;
    }

    public byte[] Payload { get; }
    public int Length => Payload.Length;

    public byte[] Serialize() => Payload;

    public static PresentationTrailer Read(
        Stream stream,
        SessionHeader sessionHeader)
    {
        var bufferLength = sessionHeader.Type switch
        {
            AssociationCommandType.RequestAssociation => AssociationRequest.Length,
            AssociationCommandType.AssociationAccepted => AssociationResponse.Length,
            AssociationCommandType.Refuse => AssociationRefuse.Length,
            AssociationCommandType.RequestRelease => ReleaseRequest.Length,
            AssociationCommandType.Released => ReleaseResponse.Length,
            AssociationCommandType.Abort => AssociationAbort.Length,
            _ => throw new ArgumentOutOfRangeException()
        };
        if (bufferLength == 0)
            return new([]);
        var buffer = new byte[bufferLength];
        stream.ReadExactLengthOrThrow(buffer, 0, bufferLength);
        return new(buffer);
    }
}