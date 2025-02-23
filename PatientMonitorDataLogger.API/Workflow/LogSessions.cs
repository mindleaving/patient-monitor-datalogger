using System.Collections.Concurrent;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class LogSessions
{
    private readonly MeasurementDataDistributor measurementDataDistributor;
    private readonly LogSessionSupervisor logSessionSupervisor;
    private readonly ConcurrentDictionary<Guid, LogSession> sessions = new();

    public LogSessions(
        MeasurementDataDistributor measurementDataDistributor,
        LogSessionSupervisor logSessionSupervisor)
    {
        this.measurementDataDistributor = measurementDataDistributor;
        this.logSessionSupervisor = logSessionSupervisor;
    }

    public List<LogSession> All => sessions.Values.ToList();

    public async Task<LogSession> CreateNew(
        LogSessionSettings settings,
        MonitorDataWriterSettings writerSettings)
    {
        var sessionId = Guid.NewGuid();
        var logSession = new LogSession(sessionId, settings, writerSettings);
        sessions.TryAdd(logSession.Id, logSession);
        logSession.NewNumericsData += DistributeNumericsData;
        logSessionSupervisor.Register(logSession);
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
        if (sessions.TryRemove(sessionId, out logSession))
        {
            logSession.NewNumericsData -= DistributeNumericsData;
            logSessionSupervisor.Unregister(logSession);
            return true;
        }

        return false;
    }

    private async void DistributeNumericsData(
        object? sender,
        NumericsData numericsData)
    {
        await measurementDataDistributor.Distribute(numericsData.LogSessionId, numericsData);
    }
}