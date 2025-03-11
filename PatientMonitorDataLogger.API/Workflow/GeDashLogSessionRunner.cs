using PatientMonitorDataLogger.API.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class GeDashLogSessionRunner : PatientMonitorLogSessionRunner
{
    public GeDashLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings settings,
        DataWriterSettings writerSettings)
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
        Stop();
    }
}