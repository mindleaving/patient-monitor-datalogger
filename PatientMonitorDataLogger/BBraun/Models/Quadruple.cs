using System.Text;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Models;

public class Quadruple : ISerializable
{
    public const char Separator = ',';

    public Quadruple(
        int relativeTimeInSeconds,
        PumpIndex address,
        string parameter,
        string? value)
    {
        RelativeTimeInSeconds = relativeTimeInSeconds;
        Address = address;
        Parameter = parameter;
        Value = value;
    }

    public int RelativeTimeInSeconds { get; }
    public PumpIndex Address { get; }
    public string Parameter { get; }
    public string? Value { get; }

    public static Quadruple Read(
        byte[] bytes)
    {
        var record = Encoding.UTF8.GetString(bytes);
        var splitted = record.Split(Separator);
        if (splitted.Length < 3 || splitted.Length > 4)
            throw new FormatException($"Response quadruple contained an unexpected number of parts ({splitted.Length})");
        var relativeTimeInSeconds = int.Parse(splitted[0]);
        var pumpAddress = PumpIndex.Parse(splitted[1]);
        var parameter = splitted[2];
        var value = splitted.Length >= 4 ? splitted[3] : null;
        return new Quadruple(relativeTimeInSeconds, pumpAddress, parameter, value);
    }

    public byte[] Serialize()
    {
        if (Value == null)
        {
            return
            [
                ..Encoding.ASCII.GetBytes(RelativeTimeInSeconds.ToString()),
                (byte)Separator,
                ..Address.Serialize(),
                (byte)Separator,
                ..Encoding.ASCII.GetBytes(Parameter)
            ];
        }

        return
        [
            ..Encoding.ASCII.GetBytes(RelativeTimeInSeconds.ToString()),
            (byte)Separator,
            ..Address.Serialize(),
            (byte)Separator,
            ..Encoding.ASCII.GetBytes(Parameter),
            (byte)Separator,
            ..Encoding.UTF8.GetBytes(Value)
        ];
    }

    public override string ToString()
    {
        return $"{RelativeTimeInSeconds}{Separator}{Address}{Separator}{Parameter}{Separator}{Value}";
    }
}