using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class PhilipsIntellivueLogSessionRunner : LogSessionRunner
{
    protected PhilipsIntellivueClient? monitorClient;
    private readonly IPatientMonitorInfo monitorInfo = new PhilipsIntellivuePatientMonitorInfo();
    private readonly PhilipsIntellivueNumericsAndWavesExtractor numericsAndWavesExtractor = new();
    private readonly PhilipsIntellivuePatientInfoExtractor patientInfoExtractor = new();
    private DateTime? connectTime;
    private readonly System.Collections.Generic.List<string> numericsTypes = new();
    private readonly System.Collections.Generic.List<string> waveTypes = new();
    
    public PhilipsIntellivueLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        MonitorDataWriterSettings writerSettings)
        : base(logSessionId, logSessionSettings, writerSettings)
    {
        if (logSessionSettings.MonitorSettings is not (PhilipsIntellivuePatientMonitorSettings or SimulatedPhilipsIntellivuePatientMonitorSettings))
            throw new ArgumentException($"Incompatible monitor settings. Expected Philips Intellivue settings but got {logSessionSettings.MonitorSettings.Type}");
    }

    protected override void InitializeImpl()
    {
        if (logSessionSettings.MonitorSettings is not PhilipsIntellivuePatientMonitorSettings philipsIntellivuePatientMonitorSettings) 
            return;
        var monitorClientSettings = PhilipsIntellivueClientSettings.CreateForPhysicalSerialPort(
            philipsIntellivuePatientMonitorSettings.SerialPortName,
            philipsIntellivuePatientMonitorSettings.SerialPortBaudRate,
            TimeSpan.FromSeconds(10), 
            PollMode.Extended);
        monitorClient = new PhilipsIntellivueClient(monitorClientSettings);
    }

    
    public override LogStatus Status
        => new(
            LogSessionId,
            monitorClient != null && monitorClient.IsConnected && monitorClient.IsListening,
            monitorInfo,
            connectTime,
            numericsTypes,
            waveTypes);
    

    protected override void StartImpl()
    {
        if (monitorClient == null)
            throw new InvalidOperationException("Monitor client is not initialized");
        monitorClient.NewMessage += HandleMonitorMessage;
        monitorClient.ConnectionStatusChanged += OnConnectionStatusChanged;
        monitorClient.Connect(
            TimeSpan.FromSeconds(1),
            ExtendedPollProfileOptions.POLL_EXT_PERIOD_NU_1SEC | ExtendedPollProfileOptions.POLL_EXT_PERIOD_RTSA | ExtendedPollProfileOptions.POLL_EXT_ENUM);
        monitorClient.StartPolling();
        monitorClient.SendPatientDemographicsRequest();
        connectTime = DateTime.UtcNow;
    }

    private void HandleMonitorMessage(
        object? sender,
        ICommandMessage message)
    {
        //WriteRawMessage(message);
        if (patientInfoExtractor.TryExtract(LogSessionId, message, out var patientInfo))
            OnPatientInfoAvailable(this, patientInfo!);
        foreach (var monitorData in numericsAndWavesExtractor.Extract(LogSessionId, message))
        {
            switch (monitorData)
            {
                case NumericsData numericsData:
                    var newNumericsMeasurementTypes = numericsData.Values.Keys.Except(numericsTypes).ToList();
                    if (newNumericsMeasurementTypes.Count > 0)
                        numericsTypes.AddRange(newNumericsMeasurementTypes);
                    numericsWriter.Write(numericsData);
                    OnNewNumericsData(this, numericsData);
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
            Path.Combine(logSessionOutputDirectory, "messages.json"),
            JsonConvert.SerializeObject(message) + Environment.NewLine);
    }

    protected override void StopImpl()
    {
        try
        {
            monitorClient?.StopPolling();
            monitorClient?.Disconnect();
        }
        catch
        {
            // Ignore
        }
        
        connectTime = null;
    }

    public override void Dispose()
    {
        Stop();
        monitorClient?.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        Stop();
        if (monitorClient != null) 
            await monitorClient.DisposeAsync();
    }

}