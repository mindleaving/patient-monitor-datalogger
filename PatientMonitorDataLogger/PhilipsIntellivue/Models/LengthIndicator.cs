using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class LengthIndicator
{
    public LengthIndicator(
        ushort length)
    {
        Length = length;
    }

    public ushort Length { get; }

    public static implicit operator LengthIndicator(ushort length) => new(length);
    public static implicit operator ushort(LengthIndicator length) => length.Length;

    public static LengthIndicator Read(
        Stream stream)
    {
        var b1 = stream.ReadByte();
        if (b1 == 0xff)
        {
            var buffer = new byte[2];
            stream.ReadExactLengthOrThrow(buffer, 0, 2);
            var length = BigEndianBitConverter.ToUInt16(buffer);
            return new(length);
        }

        return new((ushort)b1);
    }

    public byte[] Serialize()
    {
        if (Length < 0xff)
            return [(byte)Length];
        return [0xff, ..BigEndianBitConverter.GetBytes(Length)];
    }
}