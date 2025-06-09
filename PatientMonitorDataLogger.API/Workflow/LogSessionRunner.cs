using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public abstract class LogSessionRunner : ILogSessionRunner
{
    protected readonly LogSessionSettings logSessionSettings;
    protected readonly DataWriterSettings writerSettings;
    protected readonly string logSessionOutputDirectory;
    private readonly object startStopLock = new();
    protected DateTime? startTime;
    protected DateTime? lastReceivedObservationTime;
    private readonly string logSessionActiveIndicatorFilePath;

    protected LogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        DataWriterSettings writerSettings)
    {
        this.logSessionSettings = logSessionSettings;
        this.writerSettings = writerSettings;
        LogSessionId = logSessionId;
        logSessionOutputDirectory = Path.Combine(writerSettings.OutputDirectory, logSessionId.ToString());
        logSessionActiveIndicatorFilePath = GetLogSessionActiveIndicatorFilePath(logSessionId, writerSettings);
    }

    protected abstract void InitializeImpl();

    public Guid LogSessionId { get; }
    public bool IsInitialized { get; private set; }
    public bool IsRunning { get; private set; }
    public abstract LogStatus Status { get; }
    public event EventHandler<LogStatus>? StatusChanged;
    public event EventHandler<LogSessionObservations>? NewObservations;
    public event EventHandler<PatientInfo>? PatientInfoAvailable;

    public void Initialize()
    {
        if(IsInitialized)
            return;
        lock (startStopLock)
        {
            if(IsInitialized)
                return;
            if (!Directory.Exists(logSessionOutputDirectory))
                Directory.CreateDirectory(logSessionOutputDirectory);
            WriteSettings();
            InitializeImpl();
            IsInitialized = true;
        }
    }

    public void Start()
    {
        if(IsRunning)
            return;
        lock (startStopLock)
        {
            if(IsRunning)
                return;
            StartImpl();
            startTime = DateTime.UtcNow;
            IsRunning = true;
            WriteLogSessionActiveIndicatorFile();
            WriteVersionFile();
        }
    }

    private void WriteLogSessionActiveIndicatorFile()
    {
        try
        {
            File.WriteAllText(logSessionActiveIndicatorFilePath, LogSessionId.ToString());
        }
        catch
        {
            // Ignore
        }
    }

    private void WriteVersionFile()
    {
        try
        {
            File.WriteAllText(
                Path.Combine(logSessionOutputDirectory, $"version_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.txt"), 
                Assembly.GetAssembly(typeof(PhilipsIntellivueClient))!.GetName().Version!.ToString());
        }
        catch
        {
            // Ignore
        }
    }

    protected abstract void StartImpl();

    public void Stop()
    {
        if(!IsRunning)
            return;
        lock (startStopLock)
        {
            if(!IsRunning)
                return;

            IsRunning = false;
            try
            {
                File.Delete(logSessionActiveIndicatorFilePath);
            }
            catch
            {
                // Ignore
            }
            startTime = null;
            StopImpl();
        }
    }
    protected abstract void StopImpl();

    protected void OnConnectionStatusChanged(
        object? sender,
        ConnectionState connectionStatus)
    {
        StatusChanged?.Invoke(sender, Status);
    }

    protected void OnPatientInfoAvailable(
        object? sender,
        PatientInfo patientInfo)
    {
        PatientInfoAvailable?.Invoke(sender, patientInfo);
    }

    protected void OnNewObservations(
        object? sender,
        LogSessionObservations observations)
    {
        lastReceivedObservationTime = DateTime.UtcNow;
        NewObservations?.Invoke(sender, observations);
    }

    public void WritePatientInfo(
        PatientInfo patientInfo)
    {
        File.WriteAllText(
            Path.Combine(logSessionOutputDirectory, $"patient_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.json"), 
            JsonConvert.SerializeObject(patientInfo, Formatting.Indented, Constants.JsonSerializerSettings));
    }

    private void WriteSettings()
    {
        File.WriteAllText(
            Path.Combine(logSessionOutputDirectory, "settings.json"), 
            JsonConvert.SerializeObject(logSessionSettings, Formatting.Indented, Constants.JsonSerializerSettings));
    }

    public abstract void Dispose();

    public static string GetLogSessionActiveIndicatorFilePath(
        Guid logSessionId,
        DataWriterSettings writerSettings)
        => Path.Combine(writerSettings.OutputDirectory, logSessionId.ToString(), "RUNNING");
}