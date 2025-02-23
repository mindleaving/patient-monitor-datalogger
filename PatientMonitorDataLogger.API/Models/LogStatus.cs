using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Models;

public class LogStatus
{
    public LogStatus(
        Guid logSessionId,
        bool isRunning,
        IPatientMonitorInfo monitor,
        DateTime? startTime,
        List<MeasurementType> recordedNumerics,
        List<MeasurementType> recordedWaves)
    {
        LogSessionId = logSessionId;
        IsRunning = isRunning;
        Monitor = monitor;
        StartTime = startTime;
        RecordedNumerics = recordedNumerics;
        RecordedWaves = recordedWaves;
    }

    public Guid LogSessionId { get; }
    public bool IsRunning { get; }
    public IPatientMonitorInfo Monitor { get; }
    public DateTime? StartTime { get; }
    public List<MeasurementType> RecordedNumerics { get; }
    public List<MeasurementType> RecordedWaves { get; }
}