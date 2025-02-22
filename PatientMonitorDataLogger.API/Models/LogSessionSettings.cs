using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.API.Models;

public class LogSessionSettings
{
    public IPatientMonitorSettings MonitorSettings { get; set; }
    public List<MeasurementType> SelectedNumericsTypes { get; set; }
    public List<MeasurementType> SelectedWaveTypes { get; set; }
    public char CsvSeparator { get; set; } = ';';
}