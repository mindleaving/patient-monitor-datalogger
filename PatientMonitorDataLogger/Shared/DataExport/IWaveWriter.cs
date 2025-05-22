using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.Shared.DataExport;

public interface IWaveWriter : IDisposable
{
    void Start();
    void Stop();
    void Write(
        WaveData data);
}