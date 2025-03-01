using System.Collections.Concurrent;
using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;

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

    public void LoadFromDisk(
        MonitorDataWriterSettings writerSettings)
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
                if(settings.MonitorDataSettings == null)
                    continue;
            }
            catch
            {
                continue;
            }

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
        }
    }

    public async Task<LogSession> CreateNew(
        LogSessionSettings settings,
        MonitorDataWriterSettings writerSettings)
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
        logSession.NewNumericsData += DistributeNumericsData;
        logSession.PatientInfoAvailable += DistributePatientInfo;
        logSession.StatusChanged += DistributeStatusChange;
        logSessionSupervisor.Register(logSession);
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
            logSession.PatientInfoAvailable -= DistributePatientInfo;
            logSession.StatusChanged -= DistributeStatusChange;
            logSessionSupervisor.Unregister(logSession);
            return true;
        }

        return false;
    }

    private async void DistributeNumericsData(
        object? sender,
        NumericsData numericsData)
    {
        await measurementDataDistributor.Distribute(numericsData);
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
}