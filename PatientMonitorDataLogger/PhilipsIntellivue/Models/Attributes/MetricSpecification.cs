using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class MetricSpecification : ISerializable
{
    public MetricSpecification(
        RelativeTime updatePeriod,
        MetricCategory category,
        MetricAccess access,
        MetricStructure structure,
        MetricRelevance relevance)
    {
        UpdatePeriod = updatePeriod;
        Category = category;
        Access = access;
        Structure = structure;
        Relevance = relevance;
    }

    public RelativeTime UpdatePeriod { get; }
    public MetricCategory Category { get; }
    public MetricAccess Access { get; }
    public MetricStructure Structure { get; }
    public MetricRelevance Relevance { get; }

    public static MetricSpecification Read(
        BigEndianBinaryReader binaryReader)
    {
        var updatePeriod = RelativeTime.Read(binaryReader);
        var category = (MetricCategory)binaryReader.ReadUInt16();
        var access = (MetricAccess)binaryReader.ReadUInt16();
        var structure = MetricStructure.Read(binaryReader);
        var relevance = (MetricRelevance)binaryReader.ReadUInt16();
        return new(
            updatePeriod,
            category,
            access,
            structure,
            relevance);
    }

    public byte[] Serialize()
    {
        return
        [
            ..UpdatePeriod.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)Category),
            ..BigEndianBitConverter.GetBytes((ushort)Access),
            ..Structure.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)Relevance),
        ];
    }
} 