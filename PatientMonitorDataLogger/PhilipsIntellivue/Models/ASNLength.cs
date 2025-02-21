using System.Collections;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ASNLength
{
    public ASNLength(
        uint length)
    {
        Length = length;
    }
    public ASNLength(
        int length)
    {
        Length = (uint)length;
    }

    public uint Length { get; }

    public static implicit operator uint(ASNLength length) => length.Length;
    public static implicit operator int(ASNLength length) => (int)length.Length;

    public static ASNLength Read(
        Stream stream)
    {
        var b1 = (byte)stream.ReadByte();
        if (b1 <= 127)
            return new(b1);
        var bytesToRead = CountOneBits(b1) - 1; // Remove leading 1-bit, which indicates that the length is encoded with several bytes
        var buffer = new byte[bytesToRead];
        stream.ReadExactLengthOrThrow(buffer, 0, bytesToRead);
        var length = 0;
        for (int i = 0; i < buffer.Length; i++)
        {
            length += buffer[i] << (8 * (buffer.Length - 1 - i));
        }
        return new((uint)length);
    }
    public static ASNLength Read(
        BigEndianBinaryReader binaryReader)
    {
        var b1 = (byte)binaryReader.ReadByte();
        if (b1 <= 127)
            return new(b1);
        var bytesToRead = CountOneBits(b1) - 1; // Remove leading 1-bit, which indicates that the length is encoded with several bytes
        var buffer = new byte[bytesToRead];
        var bytesRead = binaryReader.Read(buffer, 0, bytesToRead);
        if (bytesRead != bytesToRead)
            throw new EndOfStreamException();
        var length = 0;
        for (int i = 0; i < buffer.Length; i++)
        {
            length += buffer[i] << (8 * (buffer.Length - 1 - i));
        }
        return new((uint)length);
    }

    private static int CountOneBits(
        byte b)
    {
        var bitArray = new BitArray(new [] { b });
        var ones = 0;
        for (int i = 0; i < bitArray.Length; i++)
        {
            if (bitArray[i])
                ones++;
        }
        return ones;
    }

    public byte[] Serialize()
    {
        if (Length < 0x80)
            return [(byte)Length];
        var bytes = BigEndianBitConverter.GetBytes(Length);
        var nonZeroBytes = bytes.SkipWhile(b => b == 0).ToArray();
        var prefix = 0x80;
        for (int i = 0; i < nonZeroBytes.Length; i++)
        {
            prefix |= 1 << i;
        }
        return [ (byte)prefix, ..nonZeroBytes];
    }
}