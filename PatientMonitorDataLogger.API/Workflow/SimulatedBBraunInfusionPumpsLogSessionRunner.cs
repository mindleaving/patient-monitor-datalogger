using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.BBraun.Models;
using PatientMonitorDataLogger.BBraun.Simulation;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.API.Workflow;

public class SimulatedBBraunInfusionPumpsLogSessionRunner : BBraunInfusionPumpsLogSessionRunner
{
    private readonly SimulatedCable simulatedCable = new();

    public SimulatedBBraunInfusionPumpsLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        DataWriterSettings writerSettings)
        : base(logSessionId, logSessionSettings, writerSettings)
    {
        if(logSessionSettings.DeviceSettings is not SimulatedBBraunInfusionPumpSettings)
            throw new ArgumentException($"Expected device setting for simulated B. Braun Space Infusion system, but got {logSessionSettings.DeviceSettings.GetType().Name}");
    }

    public SimulatedBBraunRack? SimulatedInfusionPumpRack { get; private set; }

    protected override void InitializeImpl()
    {
        var deviceSettings = (SimulatedBBraunInfusionPumpSettings)logSessionSettings.DeviceSettings;
        var clientSettings = BBraunBccClientSettings.CreateForSimulatedConnection(
            BccParticipantRole.Client,
            false,
            TimeSpan.FromSeconds(10),
            deviceSettings.PollPeriod,
            simulatedCable.End1);
        bccClient = new BBraunBccClient(clientSettings);
        List<SimulatedBBraunRackPillar> pillars = deviceSettings.PillarCount switch
        {
            1 =>
            [
                new SimulatedBBraunRackPillar(6)
            ],
            2 =>
            [
                new SimulatedBBraunRackPillar(3),
                new SimulatedBBraunRackPillar(3)
            ],
            3 =>
            [
                new SimulatedBBraunRackPillar(2),
                new SimulatedBBraunRackPillar(2),
                new SimulatedBBraunRackPillar(2)
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(deviceSettings.PillarCount))
        };
        var freeSlots = pillars.SelectMany((pillar, pillarIndex) => 
            pillar.Slots.Select(
                (slot, slotIndex) => new
                {
                    PumpIndex = new PumpIndex(pillarIndex + 1, slotIndex + 1),
                    Slot = slot
                }))
            .ToList();
        for (int pumpNumber = 0; pumpNumber < deviceSettings.PumpCount; pumpNumber++)
        {
            var slot = freeSlots[pumpNumber];
            slot.Slot.Pump = new SimulatedBBraunPerfusor();
        }
        var rackSettings = BBraunBccClientSettings.CreateForSimulatedConnection(
            BccParticipantRole.Server,
            false,
            TimeSpan.FromSeconds(10),
            deviceSettings.PollPeriod,
            simulatedCable.End2);
        SimulatedInfusionPumpRack = new SimulatedBBraunRack(
            deviceSettings.BedId,
            pillars,
            rackSettings);

        base.InitializeImpl();
    }

    protected override void StartImpl()
    {
        if (SimulatedInfusionPumpRack == null)
            throw new InvalidOperationException("Simulated infusion pump rack is not initialized");
        SimulatedInfusionPumpRack.Start();
        base.StartImpl();
    }

    protected override void StopImpl()
    {
        base.StopImpl();
        SimulatedInfusionPumpRack?.Stop();
    }

    public override void Dispose()
    {
        base.Dispose();
        simulatedCable.Dispose();
    }
}