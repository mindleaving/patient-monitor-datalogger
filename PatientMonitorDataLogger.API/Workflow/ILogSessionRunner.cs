using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow;

public interface ILogSessionRunner : IDisposable, IAsyncDisposable
{
    Guid LogSessionId { get; }
    bool IsInitialized { get; }
    bool IsRunning { get; }
    LogStatus Status { get; }
    event EventHandler<LogStatus>? StatusChanged;
    event EventHandler<NumericsData>? NewNumericsData;
    event EventHandler<PatientInfo>? PatientInfoAvailable;

    void Initialize();
    void Start();
    void Stop();

    void WritePatientInfo(
        PatientInfo patientInfo);
}