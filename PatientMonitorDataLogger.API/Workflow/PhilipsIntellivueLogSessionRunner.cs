using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.DataExport;
using PatientMonitorDataLogger.DataExport.Models;
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
    private readonly System.Collections.Generic.List<MeasurementType> numericsTypes = new();
    private readonly System.Collections.Generic.List<MeasurementType> waveTypes = new();
    private readonly INumericsWriter numericsWriter;
    private readonly Dictionary<MeasurementType, IWaveWriter> waveWriters = new();

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
        monitorClient = new PhilipsIntellivueClient(clientSettings);
        var numericsOutputFilePath = Path.Combine(writerSettings.OutputDirectory, $"numerics_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv");
        numericsWriter = new CsvNumericsWriter(numericsOutputFilePath, logSessionSettings.CsvSeparator);
    }

    public Guid LogSessionId { get; }
    public LogStatus Status
        => new(
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
        monitorClient.NewMessage += HandleMonitorMessage;
        monitorClient.ConnectionStatusChanged += HandleConnectionStatusChanged;
        monitorClient.Connect(
            TimeSpan.FromSeconds(1),
            ExtendedPollProfileOptions.POLL_EXT_PERIOD_NU_1SEC | ExtendedPollProfileOptions.POLL_EXT_PERIOD_RTSA | ExtendedPollProfileOptions.POLL_EXT_ENUM);
        monitorClient.SendPatientDemographicsRequest();
        monitorClient.StartPolling();
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
        if(patientInfoExtractor.TryExtract(message, out var patientInfo))
            PatientInfoAvailable?.Invoke(this, patientInfo!);
        foreach (var monitorData in numericsAndWavesExtractor.Extract(LogSessionId, message))
        {
            switch (monitorData)
            {
                case NumericsData numericsData:
                    var newNumericsMeasurementTypes = numericsData.Values.Keys.Except(numericsTypes).ToList();
                    if(newNumericsMeasurementTypes.Count > 0)
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

    private IWaveWriter CreateWaveWriter(
        MeasurementType measurementType)
    {
        var waveOutputFilePath = Path.Combine(writerSettings.OutputDirectory, $"{measurementType}_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv");
        IWaveWriter waveWriter = new CsvWaveWriter(measurementType, waveOutputFilePath, logSessionSettings.CsvSeparator);
        if (!waveWriters.TryAdd(measurementType, waveWriter))
        {
            waveWriter.Dispose(); // Dispose the wave writer we just created, and use the existing.
            waveWriter = waveWriters[measurementType];
        }
        return waveWriter;
    }


    public async Task Stop()
    {
        monitorClient.StopPolling();
        monitorClient.Disconnect();
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