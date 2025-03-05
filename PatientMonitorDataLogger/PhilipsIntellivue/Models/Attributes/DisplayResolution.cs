using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class DisplayResolution : ISerializable
{
    public DisplayResolution(
        byte digitsBeforeDecimalPoint,
        byte digitsAfterDecimalPoint)
    {
        DigitsBeforeDecimalPoint = digitsBeforeDecimalPoint;
        DigitsAfterDecimalPoint = digitsAfterDecimalPoint;
    }

    public byte DigitsBeforeDecimalPoint { get; }
    public byte DigitsAfterDecimalPoint { get; }

    public static DisplayResolution Read(
        BigEndianBinaryReader binaryReader)
    {
        var digitsBeforeDecimalPoint = binaryReader.ReadByte();
        var digitsAfterDecimalPoint = binaryReader.ReadByte();
        return new(digitsBeforeDecimalPoint, digitsAfterDecimalPoint);
    }

    public byte[] Serialize()
    {
        return [DigitsBeforeDecimalPoint, DigitsAfterDecimalPoint];
    }
}