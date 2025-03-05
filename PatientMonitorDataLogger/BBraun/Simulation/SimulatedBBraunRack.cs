namespace PatientMonitorDataLogger.BBraun.Simulation;

public class SimulatedBBraunRack
{
    public SimulatedBBraunRack(
        List<SimulatedBBraunRackTower> towers)
    {
        Towers = towers;
    }

    public List<SimulatedBBraunRackTower> Towers { get; }

}