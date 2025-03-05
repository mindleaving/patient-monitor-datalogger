using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class AttributeIdList : ISerializable
{
    public AttributeIdList(
        IList<OIDType> values)
    {
        Values = values;
    }

    public ushort Count => (ushort)Values.Count;
    public ushort Length => (ushort)(Values.Count * sizeof(OIDType));
    public IList<OIDType> Values { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(Count),
            ..BigEndianBitConverter.GetBytes(Length),
            ..Values.SelectMany(value => BigEndianBitConverter.GetBytes((ushort)value))
        ];
    }

    public static AttributeIdList Read(
        BigEndianBinaryReader binaryReader)
    {
        var count = binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var values = Enumerable.Range(0, count).Select(_ => (OIDType)binaryReader.ReadUInt16()).ToList();
        return new(values);
    }
}