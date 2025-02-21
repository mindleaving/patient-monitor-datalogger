using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class TextId : ISerializable
{
    public TextId(
        string label)
    {
        Label = label;
    }

    public string Label { get; }

    public static TextId Read(
        BigEndianBinaryReader binaryReader)
    {
        var label = new byte[4];
        var bytesRead = binaryReader.Read(label);
        if (bytesRead != label.Length)
            throw new EndOfStreamException();
        return new(new string(label.Select(x => (char)x).ToArray()));
    }

    public byte[] Serialize()
    {
        return Label.Take(4).Select(c => (byte)c).ToArray();
    }
}