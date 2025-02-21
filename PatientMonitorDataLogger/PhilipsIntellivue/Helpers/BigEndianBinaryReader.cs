namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class BigEndianBinaryReader
{
    private readonly Stream stream;

    public BigEndianBinaryReader(
        Stream stream)
    {
        this.stream = stream;
    }

    public ushort ReadUInt16()
    {
        var buffer = new byte[sizeof(ushort)];
        var bytesRead = stream.Read(buffer);
        if (bytesRead != sizeof(ushort))
            throw new EndOfStreamException();
        return BigEndianBitConverter.ToUInt16(buffer);
    }

    public uint ReadUInt32()
    {
        var buffer = new byte[sizeof(uint)];
        var bytesRead = stream.Read(buffer);
        if (bytesRead != sizeof(uint))
            throw new EndOfStreamException();
        return BigEndianBitConverter.ToUInt32(buffer);
    }

    public int Read(
        byte[] bytes)
    {
        return stream.Read(bytes);
    }

    public int Read(
        byte[] bytes,
        int offset,
        int length)
    {
        return stream.Read(bytes, offset, length);
    }

    public byte ReadByte()
    {
        var b = stream.ReadByte();
        if (b < 0)
            throw new EndOfStreamException();
        return (byte)b;
    }
}