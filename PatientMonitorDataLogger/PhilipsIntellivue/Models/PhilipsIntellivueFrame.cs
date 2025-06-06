﻿using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PhilipsIntellivueFrame : ISerializable
{
    public PhilipsIntellivueFrame(
        PhilipsIntellivueFrameHeader header,
        ICommandMessage userData,
        ushort checksum)
    {
        Header = header;
        UserData = userData;
        Checksum = checksum;
    }

    public PhilipsIntellivueFrameHeader Header { get; }
    public ICommandMessage UserData { get; }
    public ushort Checksum { get; }

    public static PhilipsIntellivueFrame Parse(
        byte[] buffer)
    {
        var bof = buffer[0];
        if (bof != TransparencyByteUnescapedStream.FrameStartCharacter)
            throw new ArgumentException($"Invalid frame data. Expected 0x{TransparencyByteUnescapedStream.FrameStartCharacter:X} as Beginning of Frame but got 0x{bof:X}");
        var headerBytes = buffer[1..5];
        var header = PhilipsIntellivueFrameHeader.Parse(headerBytes);
        var userDataBytes = buffer[5..(5 + header.UserDataLength)];
        var oneComplementChecksum = BitConverter.ToUInt16(buffer, 5 + header.UserDataLength); // LSB byte-order. 1's-complement.
        var checksum = (ushort)~oneComplementChecksum;
        if (!IsChecksumCorrect(headerBytes, userDataBytes, checksum))
            throw new ArgumentException("Corrupt data. Frame data doesn't match checksum");
        var eof = buffer[7 + header.UserDataLength];
        if(eof != TransparencyByteUnescapedStream.FrameEndCharacter)
            throw new ArgumentException($"Invalid frame data. Expected 0x{TransparencyByteUnescapedStream.FrameEndCharacter:X} as End of Frame but got 0x{eof:X}");

        var commandMessageReader = new CommandMessageReader();
        var userData = commandMessageReader.Read(new MemoryStream(userDataBytes));
        return new(header, userData, checksum);
    }

    private static bool IsChecksumCorrect(
        byte[] headerBytes,
        byte[] userDataBytes,
        ushort expectedChecksum)
    {
        var actualChecksum = CrcCcittFcsAlgorithm.CalculateFcs([ ..headerBytes, ..userDataBytes ]);
        return actualChecksum == expectedChecksum;
    }

    public byte[] Serialize()
    {
        byte[] frameContent =
        [
            ..Header.Serialize(),
            ..UserData.Serialize(),
            ..BitConverter.GetBytes((ushort)~Checksum) // LSB byte order. One's-complemtn.
        ];
        var escapedFrameContent = TransparencyByteUnescapedStream.Escape(frameContent);
        return
        [
            TransparencyByteUnescapedStream.FrameStartCharacter,
            ..escapedFrameContent,
            TransparencyByteUnescapedStream.FrameEndCharacter
        ];
    }
}