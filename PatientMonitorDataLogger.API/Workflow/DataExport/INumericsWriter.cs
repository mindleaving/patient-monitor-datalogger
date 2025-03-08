using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public interface INumericsWriter : IDisposable
{
    void Start();
    void Stop();
    
    void Write(
        NumericsData data);
}