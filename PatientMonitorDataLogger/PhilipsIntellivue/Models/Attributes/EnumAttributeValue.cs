using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class EnumAttributeValue<T> : ISerializable where T: struct
{
    private readonly byte[] serializedValue;

    public EnumAttributeValue(
        T value,
        byte[] serializedValue)
    {
        this.serializedValue = serializedValue;
        Value = value;
    }

    public T Value { get; }

    public byte[] Serialize()
    {
        return serializedValue;
    }

    public static EnumAttributeValue<T> Parse(
        byte[] bytes)
    {
        var value = bytes.Length switch
        {
            sizeof(uint) => (T)Enum.ToObject(typeof(T), BigEndianBitConverter.ToUInt32(bytes)),
            sizeof(ushort) => (T)Enum.ToObject(typeof(T), BigEndianBitConverter.ToUInt16(bytes)),
            sizeof(byte) => (T)Enum.ToObject(typeof(T), bytes[0]),
            _ => throw new Exception("Cannot determine size of enum")
        };
        return new EnumAttributeValue<T>(value, bytes);
    }
}