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
            PhilipsIntellivuePatientMonitorSettings philipsIntellivueSettings => new PhilipsIntellivueLogSessionRunner(philipsIntellivueSettings, writerSettings),
            _ => throw new NotSupportedException()
        };
    }

    public Guid Id { get; }
    public LogSessionSettings Settings { get; }

    public LogStatus Status => sessionRunner.Status;

    public Task Start() => sessionRunner.Start();
    public Task Stop() => sessionRunner.Stop();

    public void Dispose()
    {
        sessionRunner.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await sessionRunner.DisposeAsync();
    }
}