using System.Text.RegularExpressions;
using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.BBraun.Models;

namespace PatientMonitorDataLogger.BBraun.Simulation;

public class SimulatedBBraunRack : IDisposable
{
    private readonly BBraunBccCommunicator protocolCommunicator;
    private readonly BBraunBccMessageCreator messageCreator;
    private readonly DateTime startTime = DateTime.UtcNow;

    public SimulatedBBraunRack(
        string bedId,
        List<SimulatedBBraunRackPillar> pillars,
        BBraunBccClientSettings settings)
    {
        if (settings.Role != BccParticipantRole.Server)
            throw new ArgumentException($"Setting specifies role '{settings.Role}', but this should be a server");
        if (!settings.UseSimulatedIoDevice)
            throw new ArgumentException($"Settings indicate a physical connection, but {nameof(SimulatedBBraunRack)} only works with simulated IO devices");

        BedId = bedId;
        Pillars = pillars;
        protocolCommunicator = new BBraunBccCommunicator(settings.SimulatedIoDevice!, settings, nameof(SimulatedBBraunRack));
        protocolCommunicator.NewMessage += HandleMessage;
        messageCreator = new BBraunBccMessageCreator(settings);
    }

    public string BedId { get; }
    public List<SimulatedBBraunRackPillar> Pillars { get; }

    public void Start()
    {
        protocolCommunicator.Start();
    }

    public void Stop()
    {
        protocolCommunicator.Stop();
    }

    private void HandleMessage(
        object? sender,
        BBraunBccFrame frame)
    {
        if(frame.UserData is not BBraunBccRequest request)
            return;

        var secondsSinceStart = (uint)(DateTime.UtcNow - startTime).TotalSeconds;
        if (request.Area == "ADMIN" && request.Command == "ALIVE")
        {
            protocolCommunicator.Enqueue(messageCreator.CreateResponseMessage(BedId, [ new(secondsSinceStart, new(0,0), "GNACK", "0") ]));
            return;
        }

        if (request.Area == "ADMIN" && request.Command == "VERSION")
        {
            protocolCommunicator.Enqueue(messageCreator.CreateResponseMessage(BedId, [ new(secondsSinceStart, new(0,0), "V3.30", null) ]));
            return;
        }

        if (request.Area == "MEM" && request.Command == "GET")
        {
            for (var pillarIndex = 0; pillarIndex < Pillars.Count; pillarIndex++)
            {
                var pillar = Pillars[pillarIndex];
                for (var slotIndex = 0; slotIndex < pillar.Slots.Length; slotIndex++)
                {
                    var slot = pillar.Slots[slotIndex];
                    var pumpIndex = new PumpIndex(pillarIndex + 1, slotIndex + 1); // To 1-based index
                    ReportPumpStatus(slot.Pump, pumpIndex, slot, secondsSinceStart);
                }
            }
            return;
        }

        if (request.Area == "MEM" && Regex.IsMatch(request.Command, "^GETSLOT#[1-3][1-9A-O]$"))
        {
            var pillarNumber = int.Parse(request.Command.Substring(8, 1));
            var slotCharacter = request.Command[9];
            var pumpIndex = new PumpIndex(pillarNumber, PumpIndex.GetSlotIndexFromSlotCharacter(slotCharacter));
            if (pillarNumber > Pillars.Count)
            {
                protocolCommunicator.Enqueue(messageCreator.CreateErrorResponse(BedId, secondsSinceStart, pumpIndex, BccErrorCodes.WrongPumpAddress));
                return;
            }
            var pillar = Pillars[pillarNumber - 1];
            if (pumpIndex.Slot > pillar.Slots.Length)
            {
                protocolCommunicator.Enqueue(messageCreator.CreateErrorResponse(BedId, secondsSinceStart, pumpIndex, BccErrorCodes.WrongPumpAddress));
                return;
            }
            var slot = pillar.Slots[pumpIndex.Slot-1];
            ReportPumpStatus(slot.Pump, pumpIndex, slot, secondsSinceStart);
            return;
        }
    }

    private void ReportPumpStatus(
        SimulatedBBraunPump? pump,
        PumpIndex pumpIndex,
        SimulatedBBraunRackSlot slot,
        uint secondsSinceStart)
    {
        if (pump == null)
        {
            if (slot.WasPumpPresentAtLastRequest)
            {
                var pumpRemovedError = messageCreator.CreateErrorResponse(BedId, secondsSinceStart, pumpIndex, BccErrorCodes.PumpWasRemoved);
                protocolCommunicator.Enqueue(pumpRemovedError);
                slot.WasPumpPresentAtLastRequest = false;
            }

            return;
        }

        var pumpParameters = pump.GetParameters(pumpIndex, secondsSinceStart);
        if (!slot.WasPumpPresentAtLastRequest)
        {
            pumpParameters.Insert(0, new(secondsSinceStart, pumpIndex, "GNNEW", pump.Name));
            slot.WasPumpPresentAtLastRequest = true;
        }
        var pumpStatusMessage = messageCreator.CreateResponseMessage(BedId, pumpParameters);
        protocolCommunicator.Enqueue(pumpStatusMessage);
    }

    public void Dispose()
    {
        protocolCommunicator.NewMessage -= HandleMessage;
        protocolCommunicator.Dispose();
    }
}