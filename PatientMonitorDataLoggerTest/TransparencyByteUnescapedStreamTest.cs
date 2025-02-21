using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest;

public class TransparencyByteUnescapedStreamTest
{
    [Test]
    public void ReadReturnsAtEndOfFrame()
    {
        byte[] data =
        [
            TransparencyByteUnescapedStream.FrameStartCharacter,
            0x01,
            0x02,
            0x03,
            TransparencyByteUnescapedStream.FrameEndCharacter,
            0x04
        ];
        var baseStream = new MemoryStream(data);
        var sut = new TransparencyByteUnescapedStream(baseStream);
        var buffer = new byte[data.Length + 1];
        var bytesRead = sut.Read(buffer);
        
        Assert.That(bytesRead, Is.EqualTo(5));
        Assert.That(buffer[..bytesRead], Is.EqualTo(data[..bytesRead]));
    }

    [Test]
    public void ThrowsFrameAbortAtAbortSequence()
    {
        byte[] data =
        [
            TransparencyByteUnescapedStream.FrameStartCharacter,
            0x01,
            0x02,
            TransparencyByteUnescapedStream.EscapeCharacter,
            TransparencyByteUnescapedStream.FrameEndCharacter, // Frame abort
            TransparencyByteUnescapedStream.FrameStartCharacter,
            0x04,
            0x06
        ];
        var baseStream = new MemoryStream(data);
        var sut = new TransparencyByteUnescapedStream(baseStream);
        var buffer = new byte[data.Length + 1];

        // Test that frame abort is detected
        Assert.Throws<FrameAbortException>(() => sut.Read(buffer));

        // Test that we can continue reading after the abort
        var bytesRead = sut.Read(buffer);
        Assert.That(bytesRead, Is.EqualTo(3));
        Assert.That(buffer[..bytesRead], Is.EqualTo(data[5..]));
    }

    [Test]
    public void ThrowsFrameAbortAtUnexpectedFrameStartCharacter()
    {
        byte[] data =
        [
            TransparencyByteUnescapedStream.FrameStartCharacter,
            0x01,
            0x02,
            TransparencyByteUnescapedStream.FrameStartCharacter, // Should trigger Frame abort
            0x04,
            0x06
        ];
        var baseStream = new MemoryStream(data);
        var sut = new TransparencyByteUnescapedStream(baseStream);
        var buffer = new byte[data.Length + 1];

        // Test that frame abort is detected
        Assert.Throws<FrameAbortException>(() => sut.Read(buffer));

        // Test that we can continue reading after the abort
        var bytesRead = sut.Read(buffer);
        Assert.That(bytesRead, Is.EqualTo(3));
        Assert.That(buffer[..bytesRead], Is.EqualTo(data[3..]));
    }

    [Test]
    public void EscapedCharacterIsUnescaped()
    {
        byte[] data =
        [
            TransparencyByteUnescapedStream.FrameStartCharacter,
            0x01,
            ..TransparencyByteUnescapedStream.Escape(TransparencyByteUnescapedStream.EscapeCharacter),
            ..TransparencyByteUnescapedStream.Escape(TransparencyByteUnescapedStream.FrameStartCharacter), // Would trigger frame abort, if not escaped
            0x02,
            0x03,
            TransparencyByteUnescapedStream.FrameEndCharacter,
            0x04
        ];
        byte[] unescapedData =
        [
            TransparencyByteUnescapedStream.FrameStartCharacter,
            0x01,
            TransparencyByteUnescapedStream.EscapeCharacter,
            TransparencyByteUnescapedStream.FrameStartCharacter,
            0x02,
            0x03,
            TransparencyByteUnescapedStream.FrameEndCharacter,
        ];
        var baseStream = new MemoryStream(data);
        var sut = new TransparencyByteUnescapedStream(baseStream);
        var buffer = new byte[data.Length + 1];
        var bytesRead = sut.Read(buffer);
        
        Assert.That(bytesRead, Is.EqualTo(7));
        Assert.That(buffer[..bytesRead], Is.EqualTo(unescapedData));
    }

    [Test]
    public void DoesntReadUntilFrameStartWasFound()
    {
        byte[] data =
        [
            0x01,
            0x2,
            TransparencyByteUnescapedStream.FrameStartCharacter,
            0x03,
            0x04,
            TransparencyByteUnescapedStream.FrameEndCharacter,
            0x05
        ];
        var baseStream = new MemoryStream(data);
        var sut = new TransparencyByteUnescapedStream(baseStream);
        var buffer = new byte[data.Length + 1];
        var bytesRead = sut.Read(buffer);
        
        Assert.That(bytesRead, Is.EqualTo(4));
        Assert.That(buffer[..bytesRead], Is.EqualTo(data[2..6]));
    }

    [Test]
    public void RemembersEscapeCharacterIfAtEndOfBuffer()
    {
        byte[] data =
        [
            TransparencyByteUnescapedStream.FrameStartCharacter,
            0x03,
            0x04,
            ..TransparencyByteUnescapedStream.Escape(TransparencyByteUnescapedStream.FrameStartCharacter),
            0x05,
            0x06,
            TransparencyByteUnescapedStream.FrameEndCharacter
        ];
        byte[] unescapedData = [
            TransparencyByteUnescapedStream.FrameStartCharacter, 
            0x03,
            0x4,
            TransparencyByteUnescapedStream.FrameStartCharacter, 
            0x05, 
            0x06, 
            TransparencyByteUnescapedStream.FrameEndCharacter
        ];
        var baseStream = new MemoryStream();
        baseStream.Write(data[..4]); // Includes escape character, but not the escaped frame start character
        baseStream.Seek(0, SeekOrigin.Begin);
        var sut = new TransparencyByteUnescapedStream(baseStream);
        var buffer1 = new byte[data.Length + 1];
        var bytesRead1 = sut.Read(buffer1);

        Assert.That(bytesRead1, Is.EqualTo(3));
        Assert.That(buffer1[..bytesRead1], Is.EqualTo(data[..3]));

        // Add remaining data to stream
        baseStream.Write(data[4..]);
        baseStream.Seek(4, SeekOrigin.Begin);
        var buffer2 = new byte[data.Length + 1];
        var bytesRead2 = sut.Read(buffer2);

        Assert.That(bytesRead2, Is.EqualTo(4));
        Assert.That(buffer2[..bytesRead2], Is.EqualTo(unescapedData[3..]));

        byte[] combinedBuffers = [..buffer1[..bytesRead1], ..buffer2[..bytesRead2]];
        Assert.That(combinedBuffers, Is.EqualTo(unescapedData));
    }
}