using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class CalibrationSpecification : ISerializable
{
    public CalibrationSpecification() { }

    public CalibrationSpecification(
        IntellivueFloat lowerAbsoluteValue,
        IntellivueFloat upperAbsoluteValue,
        ushort lowerScaledValue,
        ushort upperScaledValue,
        ushort increment,
        CalibrationType calibrationType)
    {
        LowerAbsoluteValue = lowerAbsoluteValue;
        UpperAbsoluteValue = upperAbsoluteValue;
        LowerScaledValue = lowerScaledValue;
        UpperScaledValue = upperScaledValue;
        Increment = increment;
        CalibrationType = calibrationType;
    }

    public IntellivueFloat LowerAbsoluteValue { get; set; }
    public IntellivueFloat UpperAbsoluteValue { get; set; }
    public ushort LowerScaledValue { get; set; }
    public ushort UpperScaledValue { get; set; }
    public ushort Increment { get; set; }
    public CalibrationType CalibrationType { get; set; }

    public static CalibrationSpecification Read(
        BigEndianBinaryReader binaryReader)
    {
        var lowerAbsoluteValue = IntellivueFloat.Read(binaryReader);
        var upperAbsoluteValue = IntellivueFloat.Read(binaryReader);
        var lowerScaledValue = binaryReader.ReadUInt16();
        var upperScaledValue = binaryReader.ReadUInt16();
        var increment = binaryReader.ReadUInt16();
        var calibrationType = (CalibrationType)binaryReader.ReadUInt16();
        return new(
            lowerAbsoluteValue,
            upperAbsoluteValue,
            lowerScaledValue,
            upperScaledValue,
            increment,
            calibrationType);
    }

    public byte[] Serialize()
    {
        return
        [
            ..LowerAbsoluteValue.Serialize(),
            ..UpperAbsoluteValue.Serialize(),
            ..BigEndianBitConverter.GetBytes(LowerScaledValue),
            ..BigEndianBitConverter.GetBytes(UpperScaledValue),
            ..BigEndianBitConverter.GetBytes(Increment),
            ..BigEndianBitConverter.GetBytes((ushort)CalibrationType)
        ];
    }
}