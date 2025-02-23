using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class AttributeModification : ISerializable
{
    public AttributeModification(
        ModifyOperator modifyOperator,
        AttributeValueAssertion attribute)
    {
        ModifyOperator = modifyOperator;
        Attribute = attribute;
    }

    public ModifyOperator ModifyOperator { get; }
    public AttributeValueAssertion Attribute { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)ModifyOperator),
            ..Attribute.Serialize()
        ];
    }

    public static AttributeModification Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var modifyOperator = (ModifyOperator)binaryReader.ReadUInt16();
        var attribute = AttributeValueAssertion.Read(binaryReader, context);
        return new(modifyOperator, attribute);
    }
}