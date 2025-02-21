using System.Collections.Concurrent;
using PatientMonitorDataLogger.API.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class LogSessions
{
    private readonly ConcurrentDictionary<Guid, LogSession> sessions = new();

    public async Task<LogSession> CreateNew(
        LogSessionSettings settings,
        MonitorDataWriterSettings writerSettings)
    {
        var sessionId = Guid.NewGuid();
        var logSession = new LogSession(sessionId, settings, writerSettings);
        sessions.TryAdd(logSession.Id, logSession);
        return logSession;
    }

    public bool TryGet(
        Guid id,
        out LogSession? logSession)
    {
        return sessions.TryGetValue(id, out logSession);
    }

    public bool TryRemove(
        Guid sessionId,
        out LogSession? logSession)
    {
        return sessions.TryRemove(sessionId, out logSession);
    }
}