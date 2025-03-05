using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class SampleArraySpecifications : ISerializable
{
    public SampleArraySpecifications(
        ushort arraySize,
        SampleType sampleType,
        SampleArrayFlags flags)
    {
        ArraySize = arraySize;
        SampleType = sampleType;
        Flags = flags;
    }

    public ushort ArraySize { get; }
    public SampleType SampleType { get; }
    public SampleArrayFlags Flags { get; }

    public static SampleArraySpecifications Read(
        BigEndianBinaryReader binaryReader)
    {
        var arraySize = binaryReader.ReadUInt16();
        var sampleType = Models.Attributes.SampleType.Read(binaryReader);
        var flags = (SampleArrayFlags)binaryReader.ReadUInt16();
        return new(arraySize, sampleType, flags);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(ArraySize),
            ..SampleType.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)Flags),
        ];
    }
}