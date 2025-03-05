namespace PatientMonitorDataLoggerTest.BBraun;

internal static class StringMessageHelpers
{
    public static string ReplaceControlCharacters(
        string str)
    {
        return str
            .Replace("<SOH>", "\x01")
            .Replace("<STX>", "\x02")
            .Replace("<ETX>", "\x03")
            .Replace("<EOT>", "\x04")
            .Replace("<ETB>", "\x17");
    }
}