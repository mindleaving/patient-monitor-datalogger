using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.BBraun.Simulation;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLoggerTest.BBraun;

public class BBraunBccClientTest
{
    [Test]
    [Ignore("Demo code")]
    public void SimulationExample()
    {
        var simulatedCable = new SimulatedCable();
        var clientSettings = BBraunBccClientSettings
            .CreateForSimulatedConnection(
                BccParticipantRole.Client,
                useCharacterStuffing: false,
                messageRetentionPeriod: TimeSpan.FromSeconds(10), 
                pollPeriod: TimeSpan.FromSeconds(10), 
                simulatedCable.End1);
        var bccClient = new BBraunBccClient(clientSettings);

        var rackSettings = BBraunBccClientSettings.CreateForSimulatedConnection(
            BccParticipantRole.Server,
            clientSettings.UseCharacterStuffing,
            messageRetentionPeriod: TimeSpan.FromSeconds(10),
            pollPeriod: TimeSpan.FromSeconds(30), // Not used. Could be used for simulation of Cyclic Mode in the future
            simulatedCable.End2);
        var simulatedInfusionPumpRack = new SimulatedBBraunRack(
            "SpaceSystem",
            [
                new SimulatedBBraunRackPillar(3)
            ],
            rackSettings);

        // Connect
        simulatedInfusionPumpRack.Start();
        bccClient.Connect();
        bccClient.StartPolling();
        Thread.Sleep(TimeSpan.FromSeconds(25));

        // Shut down
        bccClient.StopPolling(); // If you are done using the client, call .Dispose() directly. It will call .StopPolling() and .Disconnect().
        bccClient.Disconnect();
        simulatedInfusionPumpRack.Stop();
        simulatedInfusionPumpRack.Dispose();
        bccClient.Dispose();
        simulatedCable.Dispose();
    }

    [Test]
    [Ignore("Demo code")]
    public void PhysicalConnectionExample()
    {
        var clientSettings = BBraunBccClientSettings
            .CreateForPhysicalConnection(
                BccParticipantRole.Client,
                useCharacterStuffing: false,
                pollPeriod: TimeSpan.FromSeconds(10),
                spaceStationIp: "192.168.100.41",
                spaceStationPort: 4001, 
                messageRetentionPeriod: TimeSpan.FromSeconds(10));
        var bccClient = new BBraunBccClient(clientSettings);
        bccClient.NewMessage += (sender, message) => { /* Do something with the message */ };
        bccClient.Connect();
        bccClient.StartPolling();

        while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        bccClient.StopPolling(); // If you are done using the client, call .Dispose() directly. It will call .StopPolling() and .Disconnect().
        bccClient.Disconnect();
        bccClient.Dispose();
    }
}