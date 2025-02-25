using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Models;

public class LogSessionSettings
{
    public PatientMonitorSettings MonitorSettings { get; set; }
    public List<string> SelectedNumericsTypes { get; set; }
    public List<string> SelectedWaveTypes { get; set; }
    public char CsvSeparator { get; set; } = ';';
}