using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class ScaleAndRangeSpecification : ISerializable
{
    public ScaleAndRangeSpecification(
        IntellivueFloat lowerAbsoluteValue,
        IntellivueFloat upperAbsoluteValue,
        ushort lowerScaledValue,
        ushort upperScaledValue)
    {
        LowerAbsoluteValue = lowerAbsoluteValue;
        UpperAbsoluteValue = upperAbsoluteValue;
        LowerScaledValue = lowerScaledValue;
        UpperScaledValue = upperScaledValue;
    }

    public IntellivueFloat LowerAbsoluteValue { get; }
    public IntellivueFloat UpperAbsoluteValue { get; }
    public ushort LowerScaledValue { get; }
    public ushort UpperScaledValue { get; }

    public static ScaleAndRangeSpecification Read(
        BigEndianBinaryReader binaryReader)
    {
        var lowerAbsoluteValue = IntellivueFloat.Read(binaryReader);
        var upperAbsoluteValue = IntellivueFloat.Read(binaryReader);
        var lowerScaledValue = binaryReader.ReadUInt16();
        var upperScaledValue = binaryReader.ReadUInt16();
        return new(
            lowerAbsoluteValue,
            upperAbsoluteValue,
            lowerScaledValue,
            upperScaledValue);
    }

    public byte[] Serialize()
    {
        return
        [
            ..LowerAbsoluteValue.Serialize(),
            ..UpperAbsoluteValue.Serialize(),
            ..BigEndianBitConverter.GetBytes(LowerScaledValue),
            ..BigEndianBitConverter.GetBytes(UpperScaledValue)
        ];
    }
}