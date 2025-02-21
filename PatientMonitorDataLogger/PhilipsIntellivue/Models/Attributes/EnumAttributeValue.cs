using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class EnumAttributeValue<T> : ISerializable where T: struct
{
    public EnumAttributeValue(
        T value)
    {
        Value = value;
    }

    public T Value { get; }

    public byte[] Serialize()
    {
        return Value switch
        {
            byte b => BigEndianBitConverter.GetBytes(b),
            ushort u => BigEndianBitConverter.GetBytes(u),
            uint integer => BigEndianBitConverter.GetBytes(integer),
            _ => throw new ArgumentOutOfRangeException(nameof(Value))
        };
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
        return new EnumAttributeValue<T>(value);
    }
}