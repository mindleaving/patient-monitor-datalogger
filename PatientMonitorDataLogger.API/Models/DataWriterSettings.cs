namespace PatientMonitorDataLogger.API.Models;

public class DataWriterSettings
{
    public const string AppSettingsSectionName = "Output";

    public string OutputDirectory { get; set; }
}