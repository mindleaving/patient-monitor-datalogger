namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public static class ArgsHelpers
{
    public static int GetOrDefault(
        string[] args,
        int index,
        int defaultValue)
    {
        return args.Length > index ? int.Parse(args[index]) : defaultValue;
    }
    public static string GetOrDefault(
        string[] args,
        int index,
        string defaultValue)
    {
        return args.Length > index ? args[index] : defaultValue;
    }
}