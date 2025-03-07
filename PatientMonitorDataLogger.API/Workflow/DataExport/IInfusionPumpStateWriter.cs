using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public interface IInfusionPumpStateWriter : IDisposable
{
    void Start();
    void Stop();
    
    void Write(
        InfusionPumpState data);
}