namespace PatientMonitorDataLogger.API.Models;

public class LogStatus : ILogSessionData
{
    public LogStatus(
        Guid logSessionId,
        bool isRunning,
        IPatientMonitorInfo monitor,
        DateTime? startTime,
        List<string> recordedNumerics,
        List<string> recordedWaves)
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
    public List<string> RecordedNumerics { get; }
    public List<string> RecordedWaves { get; }
}