using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SessionData
{
    public static readonly SessionData AssociationRequest = new(
    [
        0x05, 0x08, 0x13, 0x01, 0x00, 0x16, 0x01, 0x02,
        0x80, 0x00, 0x14, 0x02, 0x00, 0x02
    ]);
    public static readonly SessionData AssociationResponse = new(
    [
        0x05, 0x08, 0x13, 0x01, 0x00, 0x16, 0x01, 0x02,
        0x80, 0x00, 0x14, 0x02, 0x00, 0x02
    ]);
    public static readonly SessionData AssociationRefuse = new([0x32, 0x01, 0x00]);
    public static readonly SessionData AssociationReleaseRequest = new([]);
    public static readonly SessionData AssociationReleaseResponse = new([]);
    public static readonly SessionData AssociationAbort = new([0x11, 0x01, 0x03]);

    public SessionData(
        byte[] payload)
    {
        Payload = payload;
    }

    public byte[] Payload { get; }
    public int Length => Payload.Length;

    public byte[] Serialize() => Payload;

    public static SessionData Read(
        Stream stream,
        SessionHeader sessionHeader)
    {
        var bufferLength = sessionHeader.Type switch
        {
            AssociationCommandType.RequestAssociation => AssociationRequest.Length,
            AssociationCommandType.AssociationAccepted => AssociationResponse.Length,
            AssociationCommandType.Refuse => AssociationRefuse.Length,
            AssociationCommandType.RequestRelease => AssociationReleaseRequest.Length,
            AssociationCommandType.Released => AssociationReleaseResponse.Length,
            AssociationCommandType.Abort => AssociationAbort.Length,
            _ => throw new ArgumentOutOfRangeException(nameof(sessionHeader.Type), "Unknown session type")
        };

        var buffer = new byte[bufferLength];
        StreamHelpers.ReadExactLengthOrThrow(stream, buffer, 0, bufferLength);
        return new(buffer);
    }
}