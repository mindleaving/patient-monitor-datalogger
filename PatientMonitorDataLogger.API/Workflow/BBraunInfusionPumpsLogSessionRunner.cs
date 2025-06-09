using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.BBraun.Models;
using PatientMonitorDataLogger.Shared.DataExport;
using PatientMonitorDataLogger.Shared.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class BBraunInfusionPumpsLogSessionRunner : LogSessionRunner
{
    protected BBraunBccClient? bccClient;
    private readonly List<string> recordedParameters = new();
    private readonly IInfusionPumpStateWriter infusionPumpStateWriter;
    private RelativeTimeTranslation? relativeTimeTranslation;
    private DateTime? lastReceivedMessageTime;

    public BBraunInfusionPumpsLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        DataWriterSettings writerSettings)
        : base(logSessionId, logSessionSettings, writerSettings)
    {
        if (logSessionSettings.DeviceSettings is not (BBraunInfusionPumpSettings or SimulatedBBraunInfusionPumpSettings))
            throw new ArgumentException($"Expected B. Braun infusion pump settings, but got {logSessionSettings.DeviceSettings.GetType().Name}");

        infusionPumpStateWriter = new CsvInfusionPumpStateWriter(
            Path.Combine(logSessionOutputDirectory, $"infusion-pump-parameters_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv"),
            logSessionSettings.CsvSeparator);
    }

    protected override void InitializeImpl()
    {
        if (logSessionSettings.DeviceSettings is BBraunInfusionPumpSettings physicalDeviceSettings)
        {
            var clientSettings = BBraunBccClientSettings.CreateForPhysicalConnection(
                BccParticipantRole.Client,
                physicalDeviceSettings.Hostname,
                physicalDeviceSettings.Port,
                physicalDeviceSettings.UseCharacterStuffing,
                TimeSpan.FromSeconds(10),
                physicalDeviceSettings.PollPeriod);
            bccClient = new BBraunBccClient(clientSettings);
        }
        if (bccClient == null)
            throw new Exception("BCC client was not initialized");
        bccClient.NewMessage += OnNewMessage;
        bccClient.ConnectionStatusChanged += OnConnectionStatusChanged;
    }

    public override LogStatus Status
        => new(
            LogSessionId,
            IsRunning && bccClient != null && bccClient.IsConnected && lastReceivedObservationTime.HasValue && DateTime.UtcNow - lastReceivedObservationTime < TimeSpan.FromSeconds(60),
            new BBraunInfusionPumpInfo(bccClient?.BedId ?? "1"),
            startTime,
            recordedParameters);

    protected override void StartImpl()
    {
        if (bccClient == null)
            throw new InvalidOperationException("BCC client is not initialized");
        bccClient.Connect();
        bccClient.StartPolling();
        infusionPumpStateWriter.Start();
    }

    protected override void StopImpl()
    {
        if (bccClient == null)
            throw new InvalidOperationException("BCC client is not initialized");
        bccClient.StopPolling();
        bccClient.Disconnect();
        infusionPumpStateWriter.Stop();
    }

    public override void Dispose()
    {
        bccClient?.Dispose();
        if(bccClient != null)
        {
            bccClient.NewMessage -= OnNewMessage;
            bccClient.ConnectionStatusChanged -= OnConnectionStatusChanged;
        }
        infusionPumpStateWriter.Dispose();
    }

    private void OnNewMessage(
        object? sender,
        BBraunBccFrame frame)
    {
        if(frame.UserData is not BBraunBccResponse response)
            return;
        if(response.Quadruples.Count == 0)
            return;
        lastReceivedMessageTime = DateTime.UtcNow;
        var relativeTime = response.Quadruples[0].RelativeTimeInSeconds;
        relativeTimeTranslation ??= RelativeTimeTranslation.BBraunBccProtocol(DateTime.UtcNow, relativeTime);

        var timestamp = relativeTimeTranslation.GetAbsoluteTime(relativeTime);
        var pumps = response.Quadruples
            .Where(quadruple => quadruple.Value != null && quadruple.Value != "_NV" && quadruple.Value != "_NA")
            .GroupBy(quadruple => quadruple.Address)
            .OrderBy(g => g.Key);
        var observations = new List<Observation>();
        foreach (var pump in pumps)
        {
            var pumpParameters = pump
                .Select(quadruple => new InfusionPumpParameter(quadruple.Parameter, quadruple.Value!))
                .ToList();
            var infusionPumpState = new InfusionPumpState(timestamp, pump.Key, pumpParameters);
            infusionPumpStateWriter.Write(infusionPumpState);
            var pumpObservations = pumpParameters.Select(
                pumpParameter => new Observation(
                    timestamp,
                    $"PUMP{pump.Key}_{pumpParameter.Name}",
                    pumpParameter.Value,
                    null));
            observations.AddRange(pumpObservations.Where(x => x.ParameterName.EndsWith("_INRT")));
        }
        if(observations.Count > 0)
            OnNewObservations(this, new LogSessionObservations(LogSessionId, timestamp, observations));
    }
}