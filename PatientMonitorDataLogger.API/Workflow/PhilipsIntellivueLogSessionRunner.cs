using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.API.Workflow.DataExport;
using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class PhilipsIntellivueLogSessionRunner : ILogSessionRunner
{
    private readonly LogSessionSettings logSessionSettings;
    private readonly MonitorDataWriterSettings writerSettings;
    private readonly PhilipsIntellivueClient monitorClient;
    private readonly IPatientMonitorInfo monitorInfo;
    private readonly PhilipsIntellivueNumericsAndWavesExtractor numericsAndWavesExtractor = new();
    private readonly PhilipsIntellivuePatientInfoExtractor patientInfoExtractor = new();
    private DateTime? connectTime;
    private readonly System.Collections.Generic.List<string> numericsTypes = new();
    private readonly System.Collections.Generic.List<string> waveTypes = new();
    private readonly string logSessionOutputDirectory;
    private readonly INumericsWriter numericsWriter;
    private readonly Dictionary<string, IWaveWriter> waveWriters = new();

    public PhilipsIntellivueLogSessionRunner(
        Guid logSessionId,
        PhilipsIntellivuePatientMonitorSettings settings,
        LogSessionSettings logSessionSettings,
        MonitorDataWriterSettings writerSettings)
    {
        LogSessionId = logSessionId;
        this.logSessionSettings = logSessionSettings;
        this.writerSettings = writerSettings;
        monitorInfo = new PhilipsIntellivuePatientMonitorInfo();
        var clientSettings = PhilipsIntellivueClientSettings.CreateForPhysicalSerialPort(
            settings.SerialPortName,
            settings.SerialPortBaudRate,
            TimeSpan.FromSeconds(30));
        clientSettings.PollMode = PollMode.Extended;
        monitorClient = new PhilipsIntellivueClient(clientSettings);
        logSessionOutputDirectory = Path.Combine(writerSettings.OutputDirectory, logSessionId.ToString());
        if (!Directory.Exists(logSessionOutputDirectory))
            Directory.CreateDirectory(logSessionOutputDirectory);
        WriteSettings();
        var numericsOutputFilePath = Path.Combine(logSessionOutputDirectory, $"numerics_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv");
        numericsWriter = new CsvNumericsWriter(numericsOutputFilePath, logSessionSettings.CsvSeparator);
    }

    public Guid LogSessionId { get; }
    public LogStatus Status
        => new(
            LogSessionId,
            monitorClient.IsConnected && monitorClient.IsListening,
            monitorInfo,
            connectTime,
            numericsTypes,
            waveTypes);
    public event EventHandler<LogStatus>? StatusChanged;
    public event EventHandler<NumericsData>? NewNumericsData;
    public event EventHandler<PatientInfo>? PatientInfoAvailable;

    public async Task Start()
    {
        numericsWriter.Start();
        monitorClient.NewMessage += HandleMonitorMessage;
        monitorClient.ConnectionStatusChanged += HandleConnectionStatusChanged;
        monitorClient.Connect(
            TimeSpan.FromSeconds(1),
            ExtendedPollProfileOptions.POLL_EXT_PERIOD_NU_1SEC | ExtendedPollProfileOptions.POLL_EXT_PERIOD_RTSA | ExtendedPollProfileOptions.POLL_EXT_ENUM);
        monitorClient.StartPolling();
        monitorClient.SendPatientDemographicsRequest();
        connectTime = DateTime.UtcNow;
    }

    private void HandleConnectionStatusChanged(
        object? sender,
        MonitorConnectionChangeEventType connectionChangeEventType)
    {
        StatusChanged?.Invoke(this, Status);
    }

    private void HandleMonitorMessage(
        object? sender,
        ICommandMessage message)
    {
        WriteRawMessage(message);
        if (patientInfoExtractor.TryExtract(LogSessionId, message, out var patientInfo))
            PatientInfoAvailable?.Invoke(this, patientInfo!);
        foreach (var monitorData in numericsAndWavesExtractor.Extract(LogSessionId, message))
        {
            switch (monitorData)
            {
                case NumericsData numericsData:
                    var newNumericsMeasurementTypes = numericsData.Values.Keys.Except(numericsTypes).ToList();
                    if (newNumericsMeasurementTypes.Count > 0)
                        numericsTypes.AddRange(newNumericsMeasurementTypes);
                    numericsWriter.Write(numericsData);
                    NewNumericsData?.Invoke(this, numericsData);
                    break;
                case WaveData waveData:
                    if (!waveTypes.Contains(waveData.MeasurementType))
                        waveTypes.Add(waveData.MeasurementType);
                    if (!waveWriters.TryGetValue(waveData.MeasurementType, out var waveWriter))
                        waveWriter = CreateWaveWriter(waveData.MeasurementType);
                    waveWriter.Write(waveData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(monitorData));
            }
        }
    }

    private void WriteRawMessage(ICommandMessage message)
    {
        File.AppendAllText(
            Path.Combine(writerSettings.OutputDirectory, LogSessionId.ToString(), "messages.json"),
            JsonConvert.SerializeObject(message) + Environment.NewLine);
    }

    private IWaveWriter CreateWaveWriter(
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

    private void WriteSettings()
    {
        File.WriteAllText(
            Path.Combine(logSessionOutputDirectory, "settings.json"), 
            JsonConvert.SerializeObject(logSessionSettings));
    }

    public void WritePatientInfo(
        PatientInfo patientInfo)
    {
        File.WriteAllText(
            Path.Combine(logSessionOutputDirectory, "patient.json"), 
            JsonConvert.SerializeObject(patientInfo));
    }


    public async Task Stop()
    {
        try
        {
            monitorClient.StopPolling();
            monitorClient.Disconnect();
        }
        catch
        {
            // Ignore
        }
        try
        {
            numericsWriter.Stop();
            foreach(var waveWriter in waveWriters.Values)
            {
                waveWriter.Stop();
            }
        }
        catch
        {
            // Ignore
        }
        connectTime = null;
    }

    public void Dispose()
    {
        monitorClient.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await monitorClient.DisposeAsync();
    }

}