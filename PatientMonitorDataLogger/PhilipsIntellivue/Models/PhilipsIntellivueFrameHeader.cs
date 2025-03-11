using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PhilipsIntellivueFrameHeader : ISerializable
{
    public PhilipsIntellivueFrameHeader(
        ProtocolId protocolId,
        MessageType messageType,
        ushort userDataLength)
    {
        ProtocolId = protocolId;
        MessageType = messageType;
        UserDataLength = userDataLength;
    }

    public ProtocolId ProtocolId { get; }
    public MessageType MessageType { get; }
    public ushort UserDataLength { get; }

    public static PhilipsIntellivueFrameHeader Parse(
        byte[] buffer)
    {
        var protocolId = (ProtocolId)buffer[0];
        var messageType = (MessageType)buffer[1];
        var length = BigEndianBitConverter.ToUInt16(buffer, 2);
        return new(protocolId, messageType, length);
    }

    public byte[] Serialize()
    {
        return [(byte)ProtocolId, (byte)MessageType, ..BigEndianBitConverter.GetBytes(UserDataLength)];
    }
}