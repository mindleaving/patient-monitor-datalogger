# Medical Device Clients
This library provides clients for connecting to some medical devices.

Currently supported:
- Philips IntelliVue patient monitors

## NuGet package

Available as ```mindleaving.PatientMonitorDataLogger``` from NuGet.

## Supported devices
### Philips IntelliVue
A nearly-complete implementation of the Philips IntelliVue Serial communication protocol.

Also contains a simulated patient monitor.

How to use with real patient monitor:

```csharp
// Setup
var settings = PhilipsIntellivueClientSettings.CreateForPhysicalSerialPort(serialPortName, serialPortBaudRate, TimeSpan.FromSeconds(10), PollMode.Extended);
var philipsIntellivueCommunicator = new PhilipsIntellivueClient(settings);
philipsIntellivueCommunicator.NewMessage += (sender, message) => { /* Do something with the message, e.g. serialize to JSON and write to file */ };

// Connect
philipsIntellivueCommunicator.Connect(
    TimeSpan.FromSeconds(1), 
    ExtendedPollProfileOptions.POLL_EXT_PERIOD_NU_1SEC | ExtendedPollProfileOptions.POLL_EXT_PERIOD_RTSA | ExtendedPollProfileOptions.POLL_EXT_ENUM);
philipsIntellivueCommunicator.StartPolling(new()
{
    IncludeAlerts = true,
    IncludeNumerics = true,
    IncludeWaves = true,
    IncludePatientInfo = true
});
philipsIntellivueCommunicator.SendPatientDemographicsRequest();
while (Console.ReadKey(true).Key != ConsoleKey.Escape);

// Disconnect
philipsIntellivueCommunicator.Disconnect();
```

Use with simulated monitor:

```csharp
var simulatedCable = new SimulatedCable();
var settings = PhilipsIntellivueClientSettings.CreateForSimulatedSerialPort(simulatedCable.End1, TimeSpan.FromSeconds(10), PollMode.Extended);

// Start simulated monitor
simulatedMonitor = new SimulatedPhilipsIntellivueMonitor(simulatedCable.End2);
simulatedMonitor.Start();

philipsIntellivueCommunicator.NewMessage += (sender, message) => { /* Do something with the message, e.g. serialize to JSON and write to file */ };
philipsIntellivueCommunicator.Connect(
    TimeSpan.FromSeconds(1), 
    ExtendedPollProfileOptions.POLL_EXT_PERIOD_NU_1SEC | ExtendedPollProfileOptions.POLL_EXT_PERIOD_RTSA | ExtendedPollProfileOptions.POLL_EXT_ENUM);
philipsIntellivueCommunicator.StartPolling(new()
{
    IncludeAlerts = true,
    IncludeNumerics = true,
    IncludeWaves = true,
    IncludePatientInfo = true
});
philipsIntellivueCommunicator.SendPatientDemographicsRequest();
while (Console.ReadKey(true).Key != ConsoleKey.Escape);

// Disconnect
philipsIntellivueCommunicator.Disconnect();
simulatedMonitor!.Stop();
simulatedCable!.Dispose();
```

Refer to the Philips IntelliVue communication protocol manual for details on the message structure. Philips IntelliVue uses attributes to transport measurements. Those attributes can contain custom structures containing values, e.g. sample arrays (waves) or single float values (numerics). 

See [PhilipsIntellivueNumericsAndWavesExtractor](../PatientMonitorDataLogger.API/Workflow/PhilipsIntellivueNumericsAndWavesExtractor.cs) for code that extracts numerics and wave data from monitor messages.

### B. Braun Space Infusion Pump System
Version 3.3x of BCC communication protocol is implemented.

Includes a simulated infusion pump rack

How to use with physical connection:
```csharp
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
```

How to use with simulated rack:
```csharp
var simulatedCable = new SimulatedCable();
var clientSettings = BBraunBccClientSettings
    .CreateForSimulatedConnection(
        BccParticipantRole.Client,
        useCharacterStuffing: false,
        messageRetentionPeriod: TimeSpan.FromSeconds(10), 
        pollPeriod: TimeSpan.FromSeconds(10), 
        simulatedCable.End1);
var bccClient = new BBraunBccClient(clientSettings);
bccClient.NewMessage += (sender, message) => { /* Do something with the message */ };

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
while (Console.ReadKey(true).Key != ConsoleKey.Escape);

// Shut down
bccClient.StopPolling(); // If you are done using the client, call .Dispose() directly. It will call .StopPolling() and .Disconnect().
bccClient.Disconnect();
simulatedInfusionPumpRack.Stop();
simulatedInfusionPumpRack.Dispose();
bccClient.Dispose();
simulatedCable.Dispose();
```