using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.API.Workflow.DataExport;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public abstract class LogSessionRunner : ILogSessionRunner
{
    protected readonly LogSessionSettings logSessionSettings;
    protected readonly MonitorDataWriterSettings writerSettings;
    protected readonly string logSessionOutputDirectory;
    protected readonly INumericsWriter numericsWriter;
    protected readonly Dictionary<string, IWaveWriter> waveWriters = new();
    private readonly object startStopLock = new();

    protected LogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        MonitorDataWriterSettings writerSettings)
    {
        LogSessionId = logSessionId;
        this.logSessionSettings = logSessionSettings;
        this.writerSettings = writerSettings;
        logSessionOutputDirectory = Path.Combine(writerSettings.OutputDirectory, logSessionId.ToString());
        var numericsOutputFilePath = Path.Combine(logSessionOutputDirectory, $"numerics_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv");
        numericsWriter = new CsvNumericsWriter(numericsOutputFilePath, logSessionSettings.CsvSeparator);
    }

    protected abstract void InitializeImpl();

    public Guid LogSessionId { get; }
    public bool IsInitialized { get; private set; }
    public bool IsRunning { get; private set; }
    public abstract LogStatus Status { get; }
    public event EventHandler<LogStatus>? StatusChanged;
    public event EventHandler<NumericsData>? NewNumericsData;
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
            numericsWriter.Start();
            StartImpl();
            IsRunning = true;
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
            numericsWriter.Stop();
            foreach(var waveWriter in waveWriters.Values)
            {
                waveWriter.Stop();
            }
            StopImpl();
        }
    }
    protected abstract void StopImpl();

    protected void OnConnectionStatusChanged(
        object? sender,
        MonitorConnectionChangeEventType connectionChangeEventType)
    {
        StatusChanged?.Invoke(sender, Status);
    }

    protected void OnPatientInfoAvailable(
        object? sender,
        PatientInfo patientInfo)
    {
        PatientInfoAvailable?.Invoke(sender, patientInfo);
    }

    protected void OnNewNumericsData(
        object? sender,
        NumericsData numericsData)
    {
        NewNumericsData?.Invoke(sender, numericsData);
    }

    public void WritePatientInfo(
        PatientInfo patientInfo)
    {
        File.WriteAllText(
            Path.Combine(logSessionOutputDirectory, "patient.json"), 
            JsonConvert.SerializeObject(patientInfo, Formatting.Indented, Constants.JsonSerializerSettings));
    }

    private void WriteSettings()
    {
        File.WriteAllText(
            Path.Combine(logSessionOutputDirectory, "settings.json"), 
            JsonConvert.SerializeObject(logSessionSettings, Formatting.Indented, Constants.JsonSerializerSettings));
    }

    protected IWaveWriter CreateWaveWriter(
        string measurementType)
    {
        var waveOutputFilePath = Path.Combine(logSessionOutputDirectory, $"{measurementType}_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv");
        IWaveWriter waveWriter = new CsvWaveWriter(measurementType, waveOutputFilePath, logSessionSettings.CsvSeparator);
        if (!waveWriters.TryAdd(measurementType, waveWriter))
        {
            waveWriter.Dispose(); // Dispose the wave writer we just created, and use the existing.
            waveWriter = waveWriters[measurementType];
        }
        else
        {
            waveWriter.Start();
        }
        return waveWriter;
    }

    public abstract void Dispose();
    public abstract ValueTask DisposeAsync();
}