using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class SampleType : ISerializable
{
    public SampleType(
        byte sampleSize,
        byte significantBits)
    {
        SampleSize = sampleSize;
        SignificantBits = significantBits;
    }

    public byte SampleSize { get; }
    public byte SignificantBits { get; }

    public static SampleType Read(
        BigEndianBinaryReader binaryReader)
    {
        var sampleSize = binaryReader.ReadByte();
        var significantBits = binaryReader.ReadByte();
        return new(sampleSize, significantBits);
    }

    public byte[] Serialize()
    {
        return [ SampleSize, SignificantBits ];
    }
}