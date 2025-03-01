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
            if (logSessionsScheduledForRestart.TryRemove(logSession.Id, out _))
            {
                // Restart
                try
                {
                    Console.WriteLine($"Restarting log session {logSession.Id}");
                    logSession.Start();
                    Console.WriteLine($"Successfully restarted log session {logSession.Id}");
                }
                catch (Exception e)
                {
                    // Ignore
                    Console.WriteLine($"Could not restarted log session {logSession.Id}: {e.Message}");
                }
                continue;
            }
            if(!logSession.ShouldBeRunning)
                continue;
            if(logSession.Status.IsRunning)
                continue;

            // Schedule for restart
            try
            {
                logSession.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Scheduled log session {logSession.Id} for restart, because it stopped: {e.Message}");
            }
            logSession.ShouldBeRunning = true;
            logSessionsScheduledForRestart.TryAdd(logSession.Id, logSession);
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