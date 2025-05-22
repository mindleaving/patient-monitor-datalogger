using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class PatientMeasure : ISerializable
{
    public PatientMeasure() { }

    public PatientMeasure(
        IntellivueFloat value,
        UnitCodes unit)
    {
        Value = value;
        Unit = unit;
    }

    public IntellivueFloat Value { get; set; }
    public UnitCodes Unit { get; set; }

    public static PatientMeasure Read(
        BigEndianBinaryReader binaryReader)
    {
        var value = IntellivueFloat.Read(binaryReader);
        var unit = (UnitCodes)binaryReader.ReadUInt16();
        return new(value, unit);
    }

    public byte[] Serialize()
    {
        return
        [
            ..Value.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)Unit)
        ];
    }
}