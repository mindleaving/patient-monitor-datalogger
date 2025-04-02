namespace PatientMonitorDataLogger.Shared.Models;

public class LogSessionEvent
{
    public LogSessionEvent() { }
    public LogSessionEvent(
        string message)
    {
        Timestamp = DateTime.UtcNow;
        Message = message;
    }
    public LogSessionEvent(
        DateTime? timestamp,
        string? message)
    {
        Timestamp = timestamp;
        Message = message;
    }

    public DateTime? Timestamp { get; set; }
    public string? Message { get; set; }
}