using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.API.Workflow;

namespace PatientMonitorDataLogger.API.Models;

public class LogSession : IDisposable, IAsyncDisposable
{
    private readonly ILogSessionRunner sessionRunner;

    public LogSession(
        Guid id,
        LogSessionSettings settings,
        MonitorDataWriterSettings writerSettings)
    {
        Id = id;
        Settings = settings;
        sessionRunner = settings.MonitorSettings switch
        {
            PhilipsIntellivuePatientMonitorSettings philipsIntellivueSettings => new PhilipsIntellivueLogSessionRunner(id, philipsIntellivueSettings, settings, writerSettings),
            _ => throw new NotSupportedException()
        };
        sessionRunner.PatientInfoAvailable += UpdatePatientInfo;
        sessionRunner.StatusChanged += SessionRunner_StatusChanged;
        sessionRunner.NewNumericsData += UpdateLatestMeasurements;
    }

    public Guid Id { get; }
    public LogSessionSettings Settings { get; }
    public PatientInfo? PatientInfo { get; set; }
    public event EventHandler<PatientInfo>? PatientInfoAvailable;
    public Dictionary<string, NumericsValue> LatestMeasurements { get; } = new();
    public event EventHandler<NumericsData>? NewNumericsData;

    public bool ShouldBeRunning { get; private set; }
    public LogStatus Status => sessionRunner.Status;
    public event EventHandler<LogStatus>? StatusChanged;

    public Task Start()
    {
        ShouldBeRunning = true;
        return sessionRunner.Start();
    }

    public Task Stop()
    {
        ShouldBeRunning = false;
        return sessionRunner.Stop();
    }

    public void Dispose()
    {
        sessionRunner.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await sessionRunner.DisposeAsync();
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
        NumericsData numericsData)
    {
        foreach (var measurementType in numericsData.Values.Keys)
        {
            LatestMeasurements[measurementType] = numericsData.Values[measurementType];
        }
        NewNumericsData?.Invoke(this, numericsData);
    }
}