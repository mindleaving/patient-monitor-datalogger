using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public interface ILogSessionRunner : IDisposable
{
    Guid LogSessionId { get; }
    bool IsInitialized { get; }
    bool IsRunning { get; }
    LogStatus Status { get; }
    event EventHandler<LogStatus>? StatusChanged;
    event EventHandler<LogSessionObservations>? NewObservations;
    event EventHandler<PatientInfo>? PatientInfoAvailable;

    void Initialize();
    void Start();
    void Stop();

    void WritePatientInfo(
        PatientInfo patientInfo);
}