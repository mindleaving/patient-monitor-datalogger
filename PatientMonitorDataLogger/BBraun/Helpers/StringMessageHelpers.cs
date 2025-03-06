using System.Text;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public static class StringMessageHelpers
{
    public static string RawControlCharactersToHumanFriendly(
        byte[] bytes)
    {
        var str = Encoding.UTF8.GetString(bytes);
        return str
            .Replace("\x01", "<SOH>")
            .Replace("\x02", "<STX>")
            .Replace("\x03", "<ETX>")
            .Replace("\x04", "<EOT>")
            .Replace("\x06", "<ACK>")
            .Replace("\x15", "<NAK>")
            .Replace("\x17", "<ETB>")
            .Replace("\x1E", "<RS>");
    }

    public static string HumanFriendlyControlCharactersToRaw(
        string str)
    {
        return str
            .Replace("<SOH>", "\x01")
            .Replace("<STX>", "\x02")
            .Replace("<ETX>", "\x03")
            .Replace("<EOT>", "\x04")
            .Replace("<ACK>", "\x06")
            .Replace("<NAK>", "\x15")
            .Replace("<ETB>", "\x17")
            .Replace("<RS>", "\x1E");
    }
}