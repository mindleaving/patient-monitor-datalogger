namespace PatientMonitorDataLogger.API.Models;

public class MonitorDataWriterSettings
{
    public const string AppSettingsSectionName = "Output";

    public string OutputDirectory { get; set; }
}