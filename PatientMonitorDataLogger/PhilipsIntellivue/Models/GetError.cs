using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class GetError : ISerializable
{
    public GetError(
        ErrorStatus errorStatus,
        OIDType attributeId)
    {
        ErrorStatus = errorStatus;
        AttributeId = attributeId;
    }

    public ErrorStatus ErrorStatus { get; }
    public OIDType AttributeId { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)ErrorStatus),
            ..BigEndianBitConverter.GetBytes((ushort)AttributeId)
        ];
    }

    public static GetError Read(
        BigEndianBinaryReader binaryReader)
    {
        var errorStatus = (Models.ErrorStatus)binaryReader.ReadUInt16();
        var attributeId = (OIDType)binaryReader.ReadUInt16();
        return new(errorStatus, attributeId);
    }
}