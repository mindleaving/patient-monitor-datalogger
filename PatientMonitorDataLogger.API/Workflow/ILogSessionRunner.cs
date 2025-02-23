using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public interface ILogSessionRunner : IDisposable, IAsyncDisposable
{
    LogStatus Status { get; }
    event EventHandler<LogStatus>? StatusChanged;
    event EventHandler<NumericsData>? NewNumericsData;
    event EventHandler<PatientInfo>? PatientInfoAvailable;

    Task Start();
    Task Stop();
}