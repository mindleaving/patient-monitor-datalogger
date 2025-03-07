using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.API.Workflow.DataExport;
using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.BBraun.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class BBraunInfusionPumpsLogSessionRunner : LogSessionRunner
{
    private readonly BBraunBccClient bccClient;
    private readonly List<string> recordedParameters = new();
    private readonly IInfusionPumpStateWriter infusionPumpStateWriter;
    private RelativeTimeTranslation? relativeTimeTranslation;

    public BBraunInfusionPumpsLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        DataWriterSettings writerSettings)
        : base(logSessionId, logSessionSettings, writerSettings)
    {
        if (logSessionSettings.DeviceSettings is not BBraunInfusionPumpSettings bbraunInfusionPumpSettings)
            throw new ArgumentException($"Expected B. Braun infusion pump settings, but got {logSessionSettings.DeviceSettings.GetType().Name}");

        infusionPumpStateWriter = new CsvInfusionPumpStateWriter(
            Path.Combine(writerSettings.OutputDirectory, $"infusion-pump-parameters_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv"),
            logSessionSettings.CsvSeparator);
        var clientSettings = new BBraunBccClientSettings(
            BccParticipantRole.Client,
            bbraunInfusionPumpSettings.Hostname,
            bbraunInfusionPumpSettings.Port,
            bbraunInfusionPumpSettings.UseCharacterStuffing,
            TimeSpan.FromSeconds(10),
            bbraunInfusionPumpSettings.PollPeriod);
        bccClient = new BBraunBccClient(clientSettings);
        bccClient.NewMessage += OnNewMessage;
    }

    protected override void InitializeImpl()
    {
        // Nothing to initialize
    }

    public override LogStatus Status
        => new(
            LogSessionId,
            IsRunning,
            new BBraunInfusionPumpInfo(bccClient.BedId),
            startTime,
            recordedParameters);

    protected override void StartImpl()
    {
        bccClient.Connect();
        bccClient.StartPolling();
        infusionPumpStateWriter.Start();
    }

    protected override void StopImpl()
    {
        bccClient.StopPolling();
        bccClient.Disconnect();
        infusionPumpStateWriter.Stop();
    }

    public override void Dispose()
    {
        bccClient.Dispose();
        bccClient.NewMessage -= OnNewMessage;
        infusionPumpStateWriter.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    private void OnNewMessage(
        object? sender,
        BBraunBccFrame frame)
    {
        if(frame.UserData is not BBraunBccResponse response)
            return;
        if(response.Quadruples.Count == 0)
            return;
        var relativeTime = response.Quadruples[0].RelativeTimeInSeconds;
        relativeTimeTranslation ??= RelativeTimeTranslation.BBraunBccProtocol(DateTime.UtcNow, relativeTime);

        var timestamp = relativeTimeTranslation.GetAbsoluteTime(relativeTime);
        var pumps = response.Quadruples
            .Where(quadruple => quadruple.Value != null && quadruple.Value != "_NV" && quadruple.Value != "_NA")
            .GroupBy(quadruple => quadruple.Address)
            .OrderBy(g => g.Key);
        foreach (var pump in pumps)
        {
            var pumpParameters = pump
                .Select(quadruple => new InfusionPumpParameter(quadruple.Parameter, quadruple.Value!))
                .ToList();
            var infusionPumpState = new InfusionPumpState(timestamp, pump.Key, pumpParameters);
            infusionPumpStateWriter.Write(infusionPumpState);
        }
    }
}