using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class PatientMeasure : ISerializable
{
    public PatientMeasure(
        IntellivueFloat value,
        OIDType unit)
    {
        Value = value;
        Unit = unit;
    }

    public IntellivueFloat Value { get; }
    public OIDType Unit { get; }

    public static PatientMeasure Read(
        BigEndianBinaryReader binaryReader)
    {
        var value = IntellivueFloat.Read(binaryReader);
        var unit = (OIDType)binaryReader.ReadUInt16();
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