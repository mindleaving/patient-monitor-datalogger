using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class TransparencyByteUnescapedStream : IDisposable
{
    public const byte FrameStartCharacter = 0xC0;
    public const byte FrameEndCharacter = 0xC1;
    public const byte EscapeCharacter = 0x7D;

    private readonly Stream baseStream;

    public TransparencyByteUnescapedStream(
        Stream baseStream)
    {
        this.baseStream = baseStream;
    }

    public void Flush()
    {
        baseStream.Flush();
    }

    private byte? lastByte; // Handles edge cases, where last byte of stream is an escape character, but the following escaped byte isn't available yet.
    private bool isFrameInProgress;
    public int Read(
        byte[] buffer,
        int startIndex,
        int length)
    {
        if (startIndex + length > buffer.Length)
            throw new ArgumentOutOfRangeException(nameof(length), "Start Index and Length exceed buffer size");

        var bytesRead = 0;
        while (bytesRead < length)
        {
            var outputIndex = startIndex + bytesRead;

            var b = lastByte ?? baseStream.ReadByte();
            if (b < 0)
                return bytesRead;
            if (b == FrameStartCharacter)
            {
                if (isFrameInProgress)
                {
                    lastByte = FrameStartCharacter;
                    AbortFrame();
                }
                isFrameInProgress = true;
            }
            if(!isFrameInProgress)
                continue;
            if (b == FrameEndCharacter)
            {
                buffer[outputIndex] = (byte)b;
                bytesRead++;
                isFrameInProgress = false;
                return bytesRead;
            }
            if (b == EscapeCharacter)
            {
                b = baseStream.ReadByte();
                if (b < 0)
                {
                    lastByte = EscapeCharacter;
                    return bytesRead;
                }
                if (b == FrameEndCharacter)
                {
                    AbortFrame();
                }
                b ^= 0x20;
            }
            buffer[outputIndex] = (byte)b;
            lastByte = null;
            bytesRead++;
        }

        return bytesRead;
    }

    private void AbortFrame()
    {
        isFrameInProgress = false;
        throw new FrameAbortException();
    }

    public int Read(
        byte[] buffer)
    {
        return Read(buffer, 0, buffer.Length);
    }

    public long Seek(
        long offset,
        SeekOrigin origin)
    {
        return baseStream.Seek(offset, origin);
    }

    public void SetLength(
        long value)
    {
        throw new NotSupportedException("Setting length of this stream is not supported");
    }

    public void Write(
        byte[] buffer,
        int offset,
        int count)
    {
        if (buffer.Length < offset + count)
            throw new ArgumentException($"Buffer is too short. Writing {count} bytes was requested with offset {offset}, but buffer is only {buffer.Length} bytes long.");
        for (int inputIndex = offset; inputIndex < offset + count; inputIndex++)
        {
            var input = buffer[inputIndex];
            var output = Escape(input);
            baseStream.Write(output);
        }
    }

    public void Write(
        byte[] buffer)
    {
        Write(buffer, 0, buffer.Length);
    }

    public void WriteBeginningOfFrame() => baseStream.WriteByte(FrameStartCharacter);
    public void WriteEndOfFrame() => baseStream.WriteByte(FrameEndCharacter);
    public void WriteFrameAbort() => baseStream.Write([EscapeCharacter, FrameEndCharacter]);

    public static byte[] Escape(
        byte b)
    {
        if (b is FrameStartCharacter or FrameEndCharacter or EscapeCharacter)
            return [EscapeCharacter, (byte)(b ^ 0x20)];
        return [b];
    }

    public static byte[] Escape(
        byte[] bytes)
    {
        return bytes.SelectMany(Escape).ToArray();
    }

    public bool CanRead => baseStream.CanRead;
    public bool CanSeek => baseStream.CanSeek;
    public bool CanWrite => baseStream.CanWrite;
    public long Length => baseStream.Length;
    public long Position
    {
        get => baseStream.Position;
        set => baseStream.Position = value;
    }

    public void Close()
    {
        baseStream.Close();
    }

    public void Dispose()
    {
        baseStream.Dispose();
    }
}