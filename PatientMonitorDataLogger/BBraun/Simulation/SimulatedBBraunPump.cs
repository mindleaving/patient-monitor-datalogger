using PatientMonitorDataLogger.BBraun.Models;

namespace PatientMonitorDataLogger.BBraun.Simulation;

public class SimulatedBBraunPump
{
    public List<Quadruple> GetParameters(
        PumpIndex pumpIndex,
        int secondsSinceStart)
    {
        return [
            new(secondsSinceStart, pumpIndex, "GNMODEL", Name)
        ];
    }

    public string Name { get; set; } = "688N030003";
}