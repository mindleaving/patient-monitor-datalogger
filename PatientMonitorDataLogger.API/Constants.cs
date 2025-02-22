namespace PatientMonitorDataLogger.API;

public static class Constants
{
    public static readonly Dictionary<string, string> RepositoryPaths = new()
    {
        {"stationary-win8", @"F:\Projects\patient-monitor-datalogger"},
        {"ubuntu-stationary", @"/mnt/data/Projects/patient-monitor-datalogger"},
        {"ubuntu-laptop", @"/home/jan/Projects/patient-monitor-datalogger"},
    };
    public static string GetRepositoryPath()
    {
        return RepositoryPaths[Environment.MachineName.ToLowerInvariant()];
    }
}