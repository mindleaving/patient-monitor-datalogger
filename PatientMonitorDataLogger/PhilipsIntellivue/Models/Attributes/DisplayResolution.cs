using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class DisplayResolution : ISerializable
{
    public DisplayResolution() { }

    public DisplayResolution(
        byte digitsBeforeDecimalPoint,
        byte digitsAfterDecimalPoint)
    {
        DigitsBeforeDecimalPoint = digitsBeforeDecimalPoint;
        DigitsAfterDecimalPoint = digitsAfterDecimalPoint;
    }

    public byte DigitsBeforeDecimalPoint { get; set; }
    public byte DigitsAfterDecimalPoint { get; set; }

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