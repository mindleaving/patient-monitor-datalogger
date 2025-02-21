using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.DataExport;

public interface INumericsWriter : IDisposable, IAsyncDisposable
{
    void Write(
        NumericsData data);
}