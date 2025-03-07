namespace PatientMonitorDataLogger.API.Models.DataExport;

public class LogSessionObservations : ILogSessionData
{
    public LogSessionObservations(
        Guid logSessionId,
        DateTime timestamp,
        System.Collections.Generic.List<Observation> observations)
    {
        LogSessionId = logSessionId;
        Timestamp = timestamp;
        Observations = observations;
    }

    public Guid LogSessionId { get; }
    public DateTime Timestamp { get; }
    public System.Collections.Generic.List<Observation> Observations { get; }
}