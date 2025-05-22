using System.Text;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class IntellivueString : ISerializable
{
    public IntellivueString() { }
    public IntellivueString(
        string value)
    {
        Value = value;
    }

    public string Value { get; set; }

    public static IntellivueString Read(
        BigEndianBinaryReader binaryReader)
    {
        var length = binaryReader.ReadUInt16();
        var bytes = new byte[length];
        var bytesRead = binaryReader.Read(bytes);
        if (bytesRead != length)
            throw new EndOfStreamException();
        var str = Encoding.BigEndianUnicode.GetString(bytes);
        return new(str);
    }

    public byte[] Serialize()
    {
        var stringBytes = Encoding.BigEndianUnicode.GetBytes(Value).Concat([ (byte)0x00 ]).ToArray();
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)stringBytes.Length),
            ..stringBytes
        ];
    }

    public static string ReplacePrivateCharacters(
        string input)
    {
        return input
            .Replace("\uE145", "E")
            .Replace("\uE14C", "L")
            .Replace("\uE400", "1/min")
            .Replace("\uE401", "cmH2O")
            .Replace("\uE40D", "*")
            .Replace("\uE425", "V")
            .Replace("\uFEFF", "");
    }
}