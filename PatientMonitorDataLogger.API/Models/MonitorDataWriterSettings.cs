namespace PatientMonitorDataLogger.API.Models;

public class MonitorDataWriterSettings
{
    public string OutputDirectory { get; set; }
    public char CsvSeparator { get; set; } = ';';
}