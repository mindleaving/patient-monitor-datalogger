﻿using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class MetricSpecification : ISerializable
{
    public MetricSpecification() { }

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

    public RelativeTime UpdatePeriod { get; set; }
    public MetricCategory Category { get; set; }
    public MetricAccess Access { get; set; }
    public MetricStructure Structure { get; set; }
    public MetricRelevance Relevance { get; set; }

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