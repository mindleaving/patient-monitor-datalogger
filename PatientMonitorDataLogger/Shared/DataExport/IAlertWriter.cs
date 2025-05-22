using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.Shared.DataExport;

public interface IAlertWriter : IDisposable
{
    void Start();
    void Stop();
    
    void Write(
        Alert data);
}