using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

const bool UseSimulatedEnvironment = true;
var jsonSerializerSettings = new JsonSerializerSettings { Converters = { new StringEnumConverter() } };

PhilipsIntellivueClientSettings settings;
SimulatedSerialPortPair simulatedSerialPortPair;
SimulatedPhilipsIntellivueMonitor? simulatedMonitor;
if(!UseSimulatedEnvironment) {
    var serialPortName = ArgsHelpers.GetOrDefault(args, 0, "/dev/ttyUSB0");
    var serialPortBaudRate = ArgsHelpers.GetOrDefault(args, 1, 115200);
    settings = PhilipsIntellivueClientSettings.CreateForPhysicalSerialPort(serialPortName, serialPortBaudRate, TimeSpan.FromSeconds(10));
}
else 
{
    simulatedSerialPortPair = new SimulatedSerialPortPair();
    settings = PhilipsIntellivueClientSettings.CreateForSimulatedSerialPort(simulatedSerialPortPair.Port1, TimeSpan.FromSeconds(10));

    // Simulated monitor
    simulatedMonitor = new SimulatedPhilipsIntellivueMonitor(simulatedSerialPortPair.Port2);
    simulatedMonitor.Start();
}
settings.PollMode = PollMode.Extended;

// Communicate
var philipsIntellivueCommunicator = new PhilipsIntellivueClient(settings);
philipsIntellivueCommunicator.NewMessage += StoreMessageAsJson;
philipsIntellivueCommunicator.Connect();
philipsIntellivueCommunicator.StartPolling(); // TODO: Add polling settings (which values and waves to poll)
while (Console.ReadKey(true).Key != ConsoleKey.Escape);
philipsIntellivueCommunicator.Disconnect();
if(UseSimulatedEnvironment)
{
    simulatedMonitor.Stop();
    simulatedSerialPortPair.Dispose();
}

// Helper methods
void StoreMessageAsJson(
    object? sender,
    ICommandMessage message)
{
    File.AppendAllText("patient-monitor-datalogger-messages.json", JsonConvert.SerializeObject(message, jsonSerializerSettings) + Environment.NewLine);
}



//var geDashCommunicator = new GeDashCommunicator();
//geDashCommunicator.Start();