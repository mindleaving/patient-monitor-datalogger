using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.DataExport;

public interface IWaveWriter : IDisposable, IAsyncDisposable
{
    void Write(
        WaveData data);
}