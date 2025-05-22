using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.Shared.DataExport;

public interface IInfusionPumpStateWriter : IDisposable
{
    void Start();
    void Stop();
    
    void Write(
        InfusionPumpState data);
}