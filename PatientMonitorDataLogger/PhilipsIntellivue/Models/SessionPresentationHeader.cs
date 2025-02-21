using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SessionPresentationHeader : ISerializable
{
    public SessionPresentationHeader(
        ushort presentationContextId)
    {
        PresentationContextId = presentationContextId;
    }

    public ushort SessionId { get; } = 0xE100;
    public ushort PresentationContextId { get; }

    public byte[] Serialize()
    {
        return [
            ..BigEndianBitConverter.GetBytes(SessionId), 
            ..BigEndianBitConverter.GetBytes(PresentationContextId)
        ];
    }
}