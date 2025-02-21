using PatientMonitorDataLogger.API.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public interface ILogSessionRunner : IDisposable, IAsyncDisposable
{
    LogStatus Status { get; }
    Task Start();
    Task Stop();
}