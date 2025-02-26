using PatientMonitorDataLogger.API.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class GeDashLogSessionRunner : LogSessionRunner
{
    public GeDashLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings settings,
        MonitorDataWriterSettings writerSettings)
        : base(logSessionId, settings, writerSettings)
    {
    }

    public override LogStatus Status => throw new NotImplementedException();

    protected override void InitializeImpl()
    {
        throw new NotImplementedException();
    }

    protected override void StartImpl()
    {
        throw new NotImplementedException();
    }

    protected override void StopImpl()
    {
        throw new NotImplementedException();
    }

    public override void Dispose()
    {
    }

    public override ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}