using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Models;

public class LogSessionSettings
{
    public PatientMonitorSettings MonitorSettings { get; set; }
    public List<MeasurementType> SelectedNumericsTypes { get; set; }
    public List<MeasurementType> SelectedWaveTypes { get; set; }
    public char CsvSeparator { get; set; } = ';';
}