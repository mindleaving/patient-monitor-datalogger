using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class ScaledRange : ISerializable
{
    public ScaledRange(
        ushort lowerScaledValue,
        ushort upperScaledValue)
    {
        LowerScaledValue = lowerScaledValue;
        UpperScaledValue = upperScaledValue;
    }

    public ushort LowerScaledValue { get; }
    public ushort UpperScaledValue { get; }

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