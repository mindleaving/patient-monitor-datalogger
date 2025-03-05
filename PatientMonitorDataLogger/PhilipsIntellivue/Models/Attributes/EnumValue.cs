using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class EnumValue : ISerializable
{
    public EnumValue(
        EnumUnionChoice choice,
        ushort length,
        OIDType? enumObjectId,
        EnumObjectIdValue? enumObjectIdValue)
    {
        Choice = choice;
        Length = length;
        EnumObjectId = enumObjectId;
        EnumObjectIdValue = enumObjectIdValue;
    }

    public EnumUnionChoice Choice { get; }
    public ushort Length { get; }
    public OIDType? EnumObjectId { get; }
    public EnumObjectIdValue? EnumObjectIdValue { get; }

    public static EnumValue Read(
        BigEndianBinaryReader binaryReader)
    {
        var choice = (EnumUnionChoice)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        OIDType? enumObjectId = (choice & EnumUnionChoice.ObjectIdChosen) > 0 ? (OIDType)binaryReader.ReadUInt16() : null;
        var enumObjectIdValue = (choice & EnumUnionChoice.ObjectIdValueChosen) > 0 ? EnumObjectIdValue.Read(binaryReader) : null;
        return new(
            choice,
            length,
            enumObjectId,
            enumObjectIdValue);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)Choice),
            ..BigEndianBitConverter.GetBytes(Length),
            ..(Choice & EnumUnionChoice.ObjectIdChosen) > 0 ? BigEndianBitConverter.GetBytes((ushort)EnumObjectId!) : [],
            ..(Choice & EnumUnionChoice.ObjectIdValueChosen) > 0 ? EnumObjectIdValue!.Serialize() : []
        ];
    }
}