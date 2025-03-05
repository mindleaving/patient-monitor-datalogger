using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class UshortAttributeValue : ISerializable
{
    public UshortAttributeValue(
        ushort value)
    {
        Value = value;
    }

    public ushort Value { get; }

    public byte[] Serialize()
    {
        return BigEndianBitConverter.GetBytes(Value);
    }
}