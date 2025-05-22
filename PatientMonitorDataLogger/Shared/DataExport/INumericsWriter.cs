using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.Shared.DataExport;

public interface INumericsWriter : IDisposable
{
    void Start();
    void Stop();
    
    void Write(
        NumericsData data);
}