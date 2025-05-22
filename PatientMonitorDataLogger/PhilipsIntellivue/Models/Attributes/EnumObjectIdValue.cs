using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class EnumObjectIdValue : ISerializable
{
    public EnumObjectIdValue() { }

    public EnumObjectIdValue(
        OIDType objectId,
        IntellivueFloat numericValue,
        OIDType unitCode)
    {
        ObjectId = objectId;
        NumericValue = numericValue;
        UnitCode = unitCode;
    }

    public OIDType ObjectId { get; set; }
    public IntellivueFloat NumericValue { get; set; }
    public OIDType UnitCode { get; set; }

    public static EnumObjectIdValue Read(
        BigEndianBinaryReader binaryReader)
    {
        var objectId = (OIDType)binaryReader.ReadUInt16();
        var numericValue = IntellivueFloat.Read(binaryReader);
        var unitCode = (OIDType)binaryReader.ReadUInt16();
        return new(objectId, numericValue, unitCode);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)ObjectId),
            ..NumericValue.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)UnitCode)
        ];
    }
}