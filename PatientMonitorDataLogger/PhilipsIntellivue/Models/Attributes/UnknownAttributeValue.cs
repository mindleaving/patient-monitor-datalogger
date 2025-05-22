using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class UnknownAttributeValue : ISerializable
{
    public UnknownAttributeValue() { }

    public UnknownAttributeValue(
        byte[] data)
    {
        Data = data;
    }

    public byte[] Data { get; set; }

    public byte[] Serialize()
    {
        return Data;
    }
}