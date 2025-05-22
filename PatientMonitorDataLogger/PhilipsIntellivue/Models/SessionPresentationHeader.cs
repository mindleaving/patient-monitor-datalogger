using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SessionPresentationHeader : ISerializable
{
    public SessionPresentationHeader() { }
    public SessionPresentationHeader(
        ushort presentationContextId)
    {
        PresentationContextId = presentationContextId;
    }

    public ushort SessionId { get; } = 0xE100;
    public ushort PresentationContextId { get; set; }

    public byte[] Serialize()
    {
        return [
            ..BigEndianBitConverter.GetBytes(SessionId), 
            ..BigEndianBitConverter.GetBytes(PresentationContextId)
        ];
    }
}