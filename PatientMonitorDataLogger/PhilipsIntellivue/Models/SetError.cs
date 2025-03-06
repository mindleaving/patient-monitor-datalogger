using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SetError : ISerializable
{
    public SetError(
        ErrorStatus errorStatus,
        ModifyOperator modifyOperator,
        OIDType attributeId)
    {
        ErrorStatus = errorStatus;
        ModifyOperator = modifyOperator;
        AttributeId = attributeId;
    }

    public ErrorStatus ErrorStatus { get; }
    public ModifyOperator ModifyOperator { get; }
    public OIDType AttributeId { get; }

    public static SetError Read(
        BigEndianBinaryReader binaryReader)
    {
        var errorStatus = (ErrorStatus)binaryReader.ReadUInt16();
        var modifyOperator = (ModifyOperator)binaryReader.ReadUInt16();
        var attributeId = (OIDType)binaryReader.ReadUInt16();
        return new(errorStatus, modifyOperator, attributeId);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)ErrorStatus),
            ..BigEndianBitConverter.GetBytes((ushort)ModifyOperator),
            ..BigEndianBitConverter.GetBytes((ushort)AttributeId)
        ];
    }
}