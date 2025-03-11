using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class MetricStructure : ISerializable
{
    public MetricStructure(
        MetricStructureType type,
        byte componentCount)
    {
        Type = type;
        ComponentCount = componentCount;
    }

    public MetricStructureType Type { get; }
    public byte ComponentCount { get; }

    public static MetricStructure Read(
        BigEndianBinaryReader binaryReader)
    {
        var type = (MetricStructureType)binaryReader.ReadByte();
        var componentCount = binaryReader.ReadByte();
        return new(type, componentCount);
    }

    public byte[] Serialize()
    {
        return [(byte)Type, ComponentCount];
    }
}