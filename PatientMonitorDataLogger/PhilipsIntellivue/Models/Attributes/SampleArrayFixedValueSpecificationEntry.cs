using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class SampleArrayFixedValueSpecificationEntry : ISerializable
{
    public SampleArrayFixedValueSpecificationEntry(
        SampleArrayFixedValueId fixedValueId,
        ushort fixedValue)
    {
        FixedValueId = fixedValueId;
        FixedValue = fixedValue;
    }

    public SampleArrayFixedValueId FixedValueId { get; }
    public ushort FixedValue { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)FixedValueId),
            ..BigEndianBitConverter.GetBytes(FixedValue)
        ];
    }

    public static SampleArrayFixedValueSpecificationEntry Read(
        BigEndianBinaryReader binaryReader)
    {
        var fixedValueId = (SampleArrayFixedValueId)binaryReader.ReadUInt16();
        var fixedValue = binaryReader.ReadUInt16();
        return new(fixedValueId, fixedValue);
    }
}