using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class VisualGridEntry : ISerializable
{
    public VisualGridEntry(
        IntellivueFloat absoluteValue,
        ushort scaledValue,
        ushort level)
    {
        AbsoluteValue = absoluteValue;
        ScaledValue = scaledValue;
        Level = level;
    }

    public IntellivueFloat AbsoluteValue { get; }
    public ushort ScaledValue { get; }
    /// <summary>
    /// Relative importance. Lower is more important.
    /// </summary>
    public ushort Level { get; }

    public static VisualGridEntry Read(
        BigEndianBinaryReader binaryReader)
    {
        var absoluteValue = IntellivueFloat.Read(binaryReader);
        var scaledValue = binaryReader.ReadUInt16();
        var level = binaryReader.ReadUInt16();
        return new(absoluteValue, scaledValue, level);
    }

    public byte[] Serialize()
    {
        return
        [
            ..AbsoluteValue.Serialize(),
            ..BigEndianBitConverter.GetBytes(ScaledValue),
            ..BigEndianBitConverter.GetBytes(Level)
        ];
    }
}