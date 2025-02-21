using System.Globalization;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

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
        var isMantissaNegative = (bytes & 0x00800000) > 0;
        var mantissa = (int)(bytes & 0x00ffffff);
        if (isMantissaNegative)
            mantissa = (int)(mantissa | 0xff000000);
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
        var number = (float)(mantissa * Math.Pow(10, exponent));
        return new IntellivueFloat(number);
    }

    public byte[] Serialize()
    {
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

        var value = (int)((exponent << 24) | (mantissa & 0x00ffffff));
        return BigEndianBitConverter.GetBytes(value);
    }
}