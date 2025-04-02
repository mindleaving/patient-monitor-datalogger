using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.API.Workflow;
using PatientMonitorDataLogger.API.Workflow.DataExport;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Models;

public class LogSession : IDisposable
{
    private readonly DataWriterSettings writerSettings;
    private readonly ILogSessionRunner sessionRunner;
    private readonly CsvCustomEventWriter eventWriter;

    public LogSession(
        Guid id,
        LogSessionSettings settings,
        DataWriterSettings writerSettings)
    {
        this.writerSettings = writerSettings;
        Id = id;
        Settings = settings;
        if (settings.DeviceSettings == null)
            throw new ArgumentNullException(nameof(settings.DeviceSettings));
        if (settings.DataSettings == null)
            throw new ArgumentNullException(nameof(settings.DataSettings));
        switch (settings.DeviceSettings.DeviceType)
        {
            case MedicalDeviceType.PatientMonitor:
                var patientMonitorSettings = (PatientMonitorSettings)settings.DeviceSettings;
                sessionRunner = patientMonitorSettings.MonitorType switch
                {
                    PatientMonitorType.PhilipsIntellivue => new PhilipsIntellivueLogSessionRunner(id, settings, writerSettings),
                    PatientMonitorType.SimulatedPhilipsIntellivue => new SimulatedPhilipsIntellivueLogSessionRunner(id, settings, writerSettings),
                    PatientMonitorType.GEDash => new GeDashLogSessionRunner(id, settings, writerSettings),
                    _ => throw new ArgumentOutOfRangeException(nameof(patientMonitorSettings.MonitorType))
                };
                break;
            case MedicalDeviceType.InfusionPumps:
                var infusionPumpSettings = (InfusionPumpSettings)settings.DeviceSettings;
                sessionRunner = infusionPumpSettings.InfusionPumpType switch
                {
                    InfusionPumpType.BBraunSpace => new BBraunInfusionPumpsLogSessionRunner(id, settings, writerSettings),
                    InfusionPumpType.SimulatedBBraunSpace => new SimulatedBBraunInfusionPumpsLogSessionRunner(id, settings, writerSettings),
                    _ => throw new ArgumentOutOfRangeException(nameof(infusionPumpSettings.InfusionPumpType))
                };
                break;
            default:
                throw new NotSupportedException($"Unsupported device type {settings.DeviceSettings.DeviceType}");
        }
        sessionRunner.PatientInfoAvailable += UpdatePatientInfo;
        sessionRunner.StatusChanged += SessionRunner_StatusChanged;
        sessionRunner.NewObservations += UpdateLatestMeasurements;
        var eventsFilePath = Path.Combine(writerSettings.OutputDirectory, Id.ToString(), "events.csv");
        eventWriter = new CsvCustomEventWriter(eventsFilePath);
    }

    public Guid Id { get; }
    public LogSessionSettings Settings { get; }
    public PatientInfo? PatientInfo { get; set; }
    public event EventHandler<PatientInfo>? PatientInfoAvailable;
    public Dictionary<string, Observation> LatestObservations { get; } = new();
    public event EventHandler<LogSessionObservations>? NewObservations;

    public bool ShouldBeRunning { get; set; }
    public LogStatus Status => sessionRunner.Status;
    public event EventHandler<LogStatus>? StatusChanged;

    public void Start()
    {
        ShouldBeRunning = true;
        sessionRunner.Initialize();
        sessionRunner.Start();
        eventWriter.Start();
        LogCustomEvent(new("Started log session"));
    }

    public void Stop()
    {
        LogCustomEvent(new("Stopped log session"));
        ShouldBeRunning = false;
        sessionRunner.Stop();
        eventWriter.Stop();
    }

    public void Dispose()
    {
        Stop();
        sessionRunner.PatientInfoAvailable -= UpdatePatientInfo;
        sessionRunner.StatusChanged -= SessionRunner_StatusChanged;
        sessionRunner.NewObservations -= UpdateLatestMeasurements;
        sessionRunner.Dispose();
        eventWriter.Dispose();
    }

    private void UpdatePatientInfo(
        object? sender,
        PatientInfo newPatientInfo)
    {
        if (PatientInfo == null)
        {
            PatientInfo = newPatientInfo;
        }
        else
        {
            if (newPatientInfo.FirstName != null)
                PatientInfo.FirstName = newPatientInfo.FirstName;
            if (newPatientInfo.LastName != null)
                PatientInfo.LastName = newPatientInfo.LastName;
            if (newPatientInfo.Sex.HasValue)
                PatientInfo.Sex = newPatientInfo.Sex.Value;
            if (newPatientInfo.DateOfBirth.HasValue)
                PatientInfo.DateOfBirth = newPatientInfo.DateOfBirth.Value;
            if (newPatientInfo.PatientId != null)
                PatientInfo.PatientId = newPatientInfo.PatientId;
            if (newPatientInfo.Comment != null)
                PatientInfo.Comment = newPatientInfo.Comment;
        }
        PatientInfoAvailable?.Invoke(this, PatientInfo);
        sessionRunner.WritePatientInfo(PatientInfo);
    }

    private void SessionRunner_StatusChanged(
        object? sender, 
        LogStatus logStatus)
    {
        StatusChanged?.Invoke(this, logStatus);
    }

    private void UpdateLatestMeasurements(
        object? sender,
        LogSessionObservations observations)
    {
        foreach (var observation in observations.Observations)
        {
            LatestObservations[observation.ParameterName] = observation;
        }
        NewObservations?.Invoke(this, observations);
    }

    public void DeletePermanently()
    {
        Directory.Delete(Path.Combine(writerSettings.OutputDirectory, Id.ToString()), recursive: true);
    }

    public void LogCustomEvent(
        LogSessionEvent logSessionEvent)
    {
        eventWriter.Write(logSessionEvent);
    }
}