using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Models;

public class LogStatus : ILogSessionData
{
    public LogStatus(
        Guid logSessionId,
        bool isRunning,
        IMedicalDeviceInfo deviceInfo,
        DateTime? startTime,
        List<string> recordedParameters)
    {
        LogSessionId = logSessionId;
        IsRunning = isRunning;
        DeviceInfo = deviceInfo;
        StartTime = startTime;
        RecordedParameters = recordedParameters;
    }

    public Guid LogSessionId { get; }
    public bool IsRunning { get; }
    public IMedicalDeviceInfo DeviceInfo { get; }
    public DateTime? StartTime { get; }
    public List<string> RecordedParameters { get; }
}