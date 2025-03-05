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
var simulatedSerialPortPair = new SimulatedSerialPortPair();
var settings = PhilipsIntellivueClientSettings.CreateForSimulatedSerialPort(simulatedSerialPortPair.Port1, TimeSpan.FromSeconds(10), PollMode.Extended);

// Start simulated monitor
simulatedMonitor = new SimulatedPhilipsIntellivueMonitor(simulatedSerialPortPair.Port2);
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
simulatedSerialPortPair!.Dispose();
```

Refer to the Philips IntelliVue communication protocol manual for details on the message structure. Philips IntelliVue uses attributes to transport measurements. Those attributes can contain custom structures containing values, e.g. sample arrays (waves) or single float values (numerics). 

See [PhilipsIntellivueNumericsAndWavesExtractor](../PatientMonitorDataLogger.API/Workflow/PhilipsIntellivueNumericsAndWavesExtractor.cs) for code that extracts numerics and wave data from monitor messages.