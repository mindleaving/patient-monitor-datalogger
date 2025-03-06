using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class TextId : ISerializable
{
    public TextId(
        Labels label)
    {
        Label = label;
    }

    public Labels Label { get; }

    public static TextId Read(
        BigEndianBinaryReader binaryReader)
    {
        var label = (Labels)binaryReader.ReadUInt32();
        return new(label);
    }

    public byte[] Serialize()
    {
        return BigEndianBitConverter.GetBytes((uint)Label);
    }
}