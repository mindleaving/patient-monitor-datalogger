using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class ObservedValueArray : ISerializable
{
    public ObservedValueArray() { }

    public ObservedValueArray(
        ushort[] values)
    {
        Values = values;
    }
    public ushort[] Values { get; set; }

    public static ObservedValueArray Read(
        BigEndianBinaryReader binaryReader)
    {
        var length = binaryReader.ReadUInt16();
        var valuesBytes = new byte[length];
        var bytesRead = binaryReader.Read(valuesBytes);
        if (bytesRead != length)
            throw new EndOfStreamException();
        // According to manual, values are u_8, but looking at the data, they appear to be u_16 (ushort)
        var values = valuesBytes.Chunk(2).Select(bytePair => BigEndianBitConverter.ToUInt16(bytePair)).ToArray();
        return new(values);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)(Values.Length * sizeof(ushort))),
            ..Values.SelectMany(BigEndianBitConverter.GetBytes)
        ];
    }
}