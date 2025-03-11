namespace PatientMonitorDataLogger.BBraun.Simulation;

public class SimulatedBBraunRackPillar
{
    public SimulatedBBraunRackPillar(
        int rackFrames)
    {
        var slotCount = rackFrames * 4;
        Slots = new SimulatedBBraunRackSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            Slots[i] = new SimulatedBBraunRackSlot();
        }
    }

    public SimulatedBBraunRackSlot[] Slots { get; }
}