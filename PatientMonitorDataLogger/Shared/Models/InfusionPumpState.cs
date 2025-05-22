using PatientMonitorDataLogger.BBraun.Models;

namespace PatientMonitorDataLogger.Shared.Models;

public class InfusionPumpState
{
    public InfusionPumpState(
        DateTime timestamp,
        PumpIndex pumpIndex,
        List<InfusionPumpParameter> parameters)
    {
        Timestamp = timestamp;
        PumpIndex = pumpIndex;
        Parameters = parameters;
    }

    public DateTime Timestamp { get; }
    public PumpIndex PumpIndex { get; }
    public List<InfusionPumpParameter> Parameters { get; }
}