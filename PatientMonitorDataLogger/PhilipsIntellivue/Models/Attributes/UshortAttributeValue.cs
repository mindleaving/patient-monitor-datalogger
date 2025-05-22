using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class UshortAttributeValue : ISerializable
{
    public UshortAttributeValue() { }

    public UshortAttributeValue(
        ushort value)
    {
        Value = value;
    }

    public ushort Value { get; set; }

    public byte[] Serialize()
    {
        return BigEndianBitConverter.GetBytes(Value);
    }
}