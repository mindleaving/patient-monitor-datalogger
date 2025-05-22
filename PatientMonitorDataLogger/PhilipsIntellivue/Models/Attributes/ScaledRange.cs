using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class ScaledRange : ISerializable
{
    public ScaledRange() { }

    public ScaledRange(
        ushort lowerScaledValue,
        ushort upperScaledValue)
    {
        LowerScaledValue = lowerScaledValue;
        UpperScaledValue = upperScaledValue;
    }

    public ushort LowerScaledValue { get; set; }
    public ushort UpperScaledValue { get; set; }

    public static ScaledRange Read(
        BigEndianBinaryReader binaryReader)
    {
        var lowerScaledValue = binaryReader.ReadUInt16();
        var upperScaledValue = binaryReader.ReadUInt16();
        return new(lowerScaledValue, upperScaledValue);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(LowerScaledValue),
            ..BigEndianBitConverter.GetBytes(UpperScaledValue)
        ];
    }
}