namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class UnknownAttributeValue : ISerializable
{
    public UnknownAttributeValue(
        byte[] data)
    {
        Data = data;
    }

    public byte[] Data { get; }

    public byte[] Serialize()
    {
        return Data;
    }
}