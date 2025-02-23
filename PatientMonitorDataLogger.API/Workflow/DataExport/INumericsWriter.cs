using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public interface INumericsWriter : IDisposable, IAsyncDisposable
{
    void Start();
    void Stop();
    
    void Write(
        NumericsData data);
}