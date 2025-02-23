using System.Collections.Concurrent;
using PatientMonitorDataLogger.API.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class LogSessionSupervisor : IDisposable, IAsyncDisposable
{
    private readonly Dictionary<Guid, LogSession> logSessions = new();
    private readonly object startStopLock = new();
    private readonly Timer monitoringTimer;
    private readonly ConcurrentDictionary<Guid, LogSession> logSessionsScheduledForRestart = new();

    public LogSessionSupervisor()
    {
        monitoringTimer = new Timer(MonitorLogSessions, null, Timeout.Infinite, Timeout.Infinite);
    }

    public bool IsRunning { get; private set; }

    public void Start()
    {
        if(IsRunning)
            return;
        lock (startStopLock)
        {
            if(IsRunning)
                return;

            monitoringTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(10));
            IsRunning = true;
        }
    }

    private void MonitorLogSessions(
        object? state)
    {
        foreach (var logSession in logSessions.Values)
        {
            if(!logSession.ShouldBeRunning)
                continue;
            if(logSession.Status.IsRunning)
                continue;
            if (logSessionsScheduledForRestart.TryRemove(logSession.Id, out _))
            {
                // Restart
                logSession.Start();
            }
            else
            {
                // Schedule for restart
                logSession.Stop();
                logSessionsScheduledForRestart.TryAdd(logSession.Id, logSession);
            }
        }
    }

    public void Register(
        LogSession logSession)
    {
        logSessions.Add(logSession.Id, logSession);
    }

    public void Unregister(
        LogSession logSession)
    {
        logSessions.Remove(logSession.Id);
    }

    public void Stop()
    {
        if(!IsRunning)
            return;
        lock (startStopLock)
        {
            if(!IsRunning)
                return;
            IsRunning = false;
            monitoringTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }

    public void Dispose()
    {
        Stop();
    }

    public async ValueTask DisposeAsync()
    {
        Stop();
    }
}