using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ExtendedPollProfile : ISerializable
{
    public ExtendedPollProfile(
        ExtendedPollProfileOptions options,
        List<AttributeValueAssertion> extendedAttributes)
    {
        Options = options;
        ExtendedAttributes = extendedAttributes;
    }

    public ExtendedPollProfileOptions Options { get; }
    public List<AttributeValueAssertion> ExtendedAttributes { get; }

    public static ExtendedPollProfile Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var options = (ExtendedPollProfileOptions)binaryReader.ReadUInt32();
        var extendedAttributes = List<AttributeValueAssertion>.Read(binaryReader, x => AttributeValueAssertion.Read(x, context));
        return new(options, extendedAttributes);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((uint)Options),
            ..ExtendedAttributes.Serialize()
        ];
    }
}