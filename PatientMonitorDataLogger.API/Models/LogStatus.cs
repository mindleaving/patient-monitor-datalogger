using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.API.Models;

public class LogStatus
{
    public LogStatus(
        bool isRunning,
        IPatientMonitorInfo monitor,
        DateTime? startTime,
        List<MeasurementType> recordedNumerics,
        List<MeasurementType> recordedWaves)
    {
        IsRunning = isRunning;
        Monitor = monitor;
        StartTime = startTime;
        RecordedNumerics = recordedNumerics;
        RecordedWaves = recordedWaves;
    }

    public bool IsRunning { get; }
    public IPatientMonitorInfo Monitor { get; }
    public DateTime? StartTime { get; }
    public List<MeasurementType> RecordedNumerics { get; }
    public List<MeasurementType> RecordedWaves { get; }
}