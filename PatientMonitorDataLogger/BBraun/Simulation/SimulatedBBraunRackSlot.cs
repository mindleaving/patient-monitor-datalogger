namespace PatientMonitorDataLogger.BBraun.Simulation;

public class SimulatedBBraunRackSlot
{
    public SimulatedBBraunPump? Pump { get; set; }
    public bool WasPumpPresentAtLastRequest { get; set; } = true;
}