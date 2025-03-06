using System.Globalization;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class IntellivueFloat : ISerializable
{
    public IntellivueFloat(
        float value)
    {
        Value = value;
    }

    public float Value { get; }

    public static implicit operator float(IntellivueFloat x) => x.Value;
    public static implicit operator IntellivueFloat(float x) => new(x);

    public static IntellivueFloat Read(
        BigEndianBinaryReader binaryReader)
    {
        var bytes = binaryReader.ReadUInt32();
        var exponent = (int)(bytes >> 24);
        var isExponentNegative = (exponent & 0x80) > 0;
        if (isExponentNegative)
            exponent = (int)(exponent | 0xffffff00);
        var mantissa = (int)(bytes & 0x00ffffff);
        switch (mantissa)
        {
            case 0x7fffff:
                return new(float.NaN);
            case 0x800000:
                return new(float.NaN);
            case 0x7ffffe:
                return new(float.PositiveInfinity);
            case 0x800002:
                return new(float.NegativeInfinity);
        }
        var isMantissaNegative = (bytes & 0x00800000) > 0;
        if (isMantissaNegative)
            mantissa = (int)(mantissa | 0xff000000);
        var number = (float)(mantissa * Math.Pow(10, exponent));
        return new IntellivueFloat(number);
    }

    public byte[] Serialize()
    {
        if (float.IsNaN(Value))
            return Serialize(0, 0x7fffff);
        if (float.IsPositiveInfinity(Value))
            return Serialize(0, 0x7ffffe);
        if(float.IsNegativeInfinity(Value))
            return Serialize(0, 0x800002);
        int mantissa, exponent;
        var floatAsString = Value.ToString(CultureInfo.InvariantCulture);
        var decimalPointPosition = floatAsString.IndexOf('.');
        if (decimalPointPosition < 0)
        {
            mantissa = (int)Value;
            exponent = 0;
        }
        else
        {
            var digitsBeforeDecimalPoint = decimalPointPosition;
            exponent = digitsBeforeDecimalPoint - 6;
            mantissa = (int)Math.Round(Value * Math.Pow(10, -exponent));
        }

        return Serialize(exponent, mantissa);
    }

    private static byte[] Serialize(
        int exponent,
        int mantissa)
    {
        var value = (exponent << 24) | (mantissa & 0x00ffffff);
        return BigEndianBitConverter.GetBytes(value);
    }
}