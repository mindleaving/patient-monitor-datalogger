using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class PhilipsIntellivueLogSessionRunner : PatientMonitorLogSessionRunner
{
    protected readonly PatientMonitorSettings monitorSettings;
    protected readonly PatientMonitorDataSettings dataSettings;
    protected PhilipsIntellivueClient? monitorClient;
    private readonly IPatientMonitorInfo monitorInfo = new PhilipsIntellivuePatientMonitorInfo();
    private readonly PhilipsIntellivueNumericsAndWavesExtractor numericsAndWavesExtractor = new();
    private readonly PhilipsIntellivuePatientInfoExtractor patientInfoExtractor = new();
    private readonly System.Collections.Generic.List<string> numericsTypes = new();
    private readonly System.Collections.Generic.List<string> waveTypes = new();
    
    public PhilipsIntellivueLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        DataWriterSettings writerSettings)
        : base(logSessionId, logSessionSettings, writerSettings)
    {
        if (logSessionSettings.DeviceSettings is not (PhilipsIntellivuePatientMonitorSettings or SimulatedPhilipsIntellivuePatientMonitorSettings))
            throw new ArgumentException($"Incompatible monitor settings. Expected Philips Intellivue settings but got {logSessionSettings.DeviceSettings.GetType().Name}");
        monitorSettings = (PatientMonitorSettings)logSessionSettings.DeviceSettings;
        if (logSessionSettings.DataSettings is not PatientMonitorDataSettings patientMonitorDataSettings)
            throw new ArgumentException($"Expected patient monitor data settings, but got {logSessionSettings.DataSettings.GetType().Name}");
        dataSettings = patientMonitorDataSettings;
    }

    protected override void InitializeImpl()
    {
        if (monitorSettings is not PhilipsIntellivuePatientMonitorSettings philipsIntellivuePatientMonitorSettings) 
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
            startTime,
            numericsTypes.Concat(waveTypes).ToList());
    

    protected override void StartImpl()
    {
        if (monitorClient == null)
            throw new InvalidOperationException("Monitor client is not initialized");
        monitorClient.NewMessage += HandleMonitorMessage;
        monitorClient.ConnectionStatusChanged += OnConnectionStatusChanged;
        var pollOptions = ExtendedPollProfileOptions.None;
        if (dataSettings.IncludeNumerics)
            pollOptions |= ExtendedPollProfileOptions.POLL_EXT_PERIOD_NU_1SEC;
        if (dataSettings.IncludeWaves)
            pollOptions |= ExtendedPollProfileOptions.POLL_EXT_PERIOD_RTSA;
        monitorClient.Connect(TimeSpan.FromSeconds(1), pollOptions);
        monitorClient.StartPolling(dataSettings);
        if (dataSettings.IncludeWaves && dataSettings.SelectedWaveTypes.Any())
        {
            var wavePriorityList = BuildWavePriorityListFromSettings(dataSettings);
            monitorClient.SetWavePriorityList(wavePriorityList);
        }
        if(dataSettings.IncludePatientInfo)
            monitorClient.SendPatientDemographicsRequest();
    }

    private static ICollection<Labels> BuildWavePriorityListFromSettings(
        PatientMonitorDataSettings monitorDataSettings)
    {
        if (monitorDataSettings.SelectedWaveTypes.Count == 0)
        {
            return
            [
                Waves.NLS_NOM_PRESS_BLD_ART_ABP.Label,
                Waves.NLS_NOM_PRESS_BLD_ART.Label,
                Waves.NLS_NOM_PULS_OXIM_PLETH.Label,
                Waves.NLS_NOM_RESP.Label
            ];
        }
        var waveLabels = new System.Collections.Generic.List<Labels>();
        foreach (var selectedWaveType in monitorDataSettings.SelectedWaveTypes)
        {
            switch (selectedWaveType)
            {
                case WaveType.EcgDefault:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL);
                    break;
                case WaveType.EcgI:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL_I);
                    break;
                case WaveType.EcgII:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL_II);
                    break;
                case WaveType.EcgIII:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL_III);
                    break;
                case WaveType.EcgV1:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL_V1);
                    break;
                case WaveType.EcgV2:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL_V2);
                    break;
                case WaveType.EcgV3:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL_V3);
                    break;
                case WaveType.EcgV4:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL_V4);
                    break;
                case WaveType.EcgV5:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL_V5);
                    break;
                case WaveType.EcgV6:
                    waveLabels.Add(Labels.NLS_NOM_ECG_ELEC_POTL_V6);
                    break;
                case WaveType.Pleth:
                    waveLabels.Add(Labels.NLS_NOM_PULS_OXIM_PLETH);
                    break;
                case WaveType.Pleth2:
                    waveLabels.Add(Labels.NLS_NOM_EMFC_PLETH2);
                    break;
                case WaveType.ArterialBloodPressure:
                    waveLabels.Add(Labels.NLS_NOM_PRESS_BLD_ART_ABP);
                    waveLabels.Add(Labels.NLS_NOM_PRESS_BLD_ART);
                    break;
                case WaveType.CO2:
                    waveLabels.Add(Labels.NLS_NOM_AWAY_CO2);
                    break;
                case WaveType.Respiration:
                    waveLabels.Add(Labels.NLS_NOM_RESP);
                    break;
            }
        }

        return waveLabels;
    }

    private void HandleMonitorMessage(
        object? sender,
        ICommandMessage message)
    {
        WriteRawMessage(message);
        if (patientInfoExtractor.TryExtract(LogSessionId, message, out var patientInfo))
            OnPatientInfoAvailable(this, patientInfo!);
        foreach (var monitorData in numericsAndWavesExtractor.Extract(message))
        {
            switch (monitorData)
            {
                case NumericsData numericsData:
                    var newNumericsMeasurementTypes = numericsData.Values.Keys.Except(numericsTypes).ToList();
                    if (newNumericsMeasurementTypes.Count > 0)
                        numericsTypes.AddRange(newNumericsMeasurementTypes);
                    numericsWriter.Write(numericsData);
                    var logSessionObservations = new LogSessionObservations(
                        LogSessionId, 
                        numericsData.Timestamp, 
                        numericsData.Values
                            .Select(kvp => new Observation(kvp.Value.Timestamp, kvp.Key, kvp.Value.Value.ToString("F1"), kvp.Value.Unit))
                            .ToList());
                    OnNewObservations(this, logSessionObservations);
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
            JsonConvert.SerializeObject(message, Constants.JsonSerializerSettings) + Environment.NewLine);
    }

    protected override void StopImpl()
    {
        try
        {
            monitorClient?.StopPolling();
            monitorClient?.Disconnect();
            if(monitorClient != null)
            {
                monitorClient.NewMessage -= HandleMonitorMessage;
                monitorClient.ConnectionStatusChanged -= OnConnectionStatusChanged;
            }
        }
        catch
        {
            // Ignore
        }
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