using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class List<T> : ISerializable where T: ISerializable
{
    public List() { }

    public List(
        System.Collections.Generic.List<T> values)
    {
        Values = values;
    }

    public ushort Count => (ushort)Values.Count;
    public ushort Length => (ushort)Values.Sum(x => x.Serialize().Length);
    public System.Collections.Generic.List<T> Values { get; set; }

    public byte[] Serialize()
    {
        var valueBytes = Values.SelectMany(value => value.Serialize()).ToList();
        return
        [
            ..BigEndianBitConverter.GetBytes(Count),
            ..BigEndianBitConverter.GetBytes((ushort)valueBytes.Count),
            ..valueBytes
        ];
    }

    public static List<T> Read(
        BigEndianBinaryReader binaryReader,
        Func<BigEndianBinaryReader, T> itemReader)
    {
        var count = binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var values = new System.Collections.Generic.List<T>();
        for (int i = 0; i < count; i++)
        {
            var value = itemReader(binaryReader);
            values.Add(value);
        }
        return new(values);
    }
}