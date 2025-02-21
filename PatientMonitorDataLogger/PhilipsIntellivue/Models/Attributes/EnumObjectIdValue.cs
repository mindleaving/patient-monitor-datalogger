using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class EnumObjectIdValue : ISerializable
{
    public EnumObjectIdValue(
        OIDType objectId,
        IntellivueFloat numericValue,
        OIDType unitCode)
    {
        ObjectId = objectId;
        NumericValue = numericValue;
        UnitCode = unitCode;
    }

    public OIDType ObjectId { get; }
    public IntellivueFloat NumericValue { get; }
    public OIDType UnitCode { get; }

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