using PatientMonitorDataLogger.BBraun.Models;

namespace PatientMonitorDataLogger.BBraun.Simulation;

public abstract class SimulatedBBraunPump
{
    public List<Quadruple> GetParameters(
        PumpIndex pumpIndex,
        uint secondsSinceStart)
    {
        return [
            new(secondsSinceStart, pumpIndex, "GNMODEL", Name),
            new(secondsSinceStart, pumpIndex, "INRT", InfusionRate.ToString("F1"))
        ];
    }

    public string Name { get; set; } = "688N030003";
    public double InfusionRate { get; set; } = 20;
}