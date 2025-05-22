using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class LogSessions : IHostedService
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

    public void LoadFromDisk(
        DataWriterSettings writerSettings)
    {
        if(!Directory.Exists(writerSettings.OutputDirectory))
            return;
        var logSessionDirectories = Directory.EnumerateDirectories(writerSettings.OutputDirectory)
            .Where(directoryName => Guid.TryParse(new DirectoryInfo(directoryName).Name, out _));
        foreach (var logSessionDirectory in logSessionDirectories)
        {
            var logSessionId = Guid.Parse(new DirectoryInfo(logSessionDirectory).Name);
            var settingsFilePath = Path.Combine(logSessionDirectory, Constants.SettingsFileName);
            if(!File.Exists(settingsFilePath))
                continue;
            LogSessionSettings settings;
            try
            {
                var json = File.ReadAllText(settingsFilePath);
                settings = JsonConvert.DeserializeObject<LogSessionSettings>(json)!;
            }
            catch
            {
                continue;
            }

            if(settings.DeviceSettings == null || settings.DataSettings == null)
                continue;
            var logSession = new LogSession(logSessionId, settings, writerSettings);
            var patientInfoFilePath = Path.Combine(logSessionDirectory, Constants.PatientInfoFileName);
            if (File.Exists(patientInfoFilePath))
            {
                try
                {
                    var json = File.ReadAllText(patientInfoFilePath);
                    var patientInfo = JsonConvert.DeserializeObject<PatientInfo>(json);
                    logSession.PatientInfo = patientInfo;
                }
                catch
                {
                    // Ignore
                }
            }
            AddAndSetupLogSession(logSession);
            if(File.Exists(LogSessionRunner.GetLogSessionActiveIndicatorFilePath(logSessionId, writerSettings)))
            {
                try
                {
                    logSession.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not start log session {logSessionId}: " + e.Message);
                }
            }
        }
    }

    public async Task<LogSession> CreateNew(
        LogSessionSettings settings,
        DataWriterSettings writerSettings)
    {
        var sessionId = Guid.NewGuid();
        var logSession = new LogSession(sessionId, settings, writerSettings);
        AddAndSetupLogSession(logSession);
        return logSession;
    }

    private void AddAndSetupLogSession(
        LogSession logSession)
    {
        sessions.TryAdd(logSession.Id, logSession);
        logSession.NewObservations += DistributeObservations;
        logSession.PatientInfoAvailable += DistributePatientInfo;
        logSession.StatusChanged += DistributeStatusChange;
        logSessionSupervisor.Register(logSession);
    }

    public bool TryGet(
        Guid id,
        [NotNullWhen(true)] out LogSession? logSession)
    {
        return sessions.TryGetValue(id, out logSession);
    }

    public bool TryRemove(
        Guid sessionId,
        out LogSession? logSession)
    {
        if (sessions.TryRemove(sessionId, out logSession))
        {
            logSession.NewObservations -= DistributeObservations;
            logSession.PatientInfoAvailable -= DistributePatientInfo;
            logSession.StatusChanged -= DistributeStatusChange;
            logSessionSupervisor.Unregister(logSession);
            return true;
        }

        return false;
    }

    private async void DistributeObservations(
        object? sender,
        LogSessionObservations observations)
    {
        await measurementDataDistributor.Distribute(observations);
    }

    private async void DistributePatientInfo(
        object? sender,
        PatientInfo patientInfo)
    {
        await measurementDataDistributor.Distribute(patientInfo);
    }

    private async void DistributeStatusChange(
        object? sender,
        LogStatus logStatus)
    {
        await measurementDataDistributor.Distribute(logStatus);
    }

    public Task StartAsync(
        CancellationToken cancellationToken)
    {
        // Nothing to do
        return Task.CompletedTask;
    }

    public Task StopAsync(
        CancellationToken cancellationToken)
    {
        // Nothing to do
        return Task.CompletedTask;
    }
}