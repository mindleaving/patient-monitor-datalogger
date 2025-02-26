using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

var useSimulatedEnvironment = args.Length > 0 && args[0].ToLower() == "simulate";
var jsonSerializerSettings = new JsonSerializerSettings { Converters = { new StringEnumConverter() } };

PhilipsIntellivueClientSettings settings;
SimulatedSerialPortPair? simulatedSerialPortPair = null;
SimulatedPhilipsIntellivueMonitor? simulatedMonitor = null;
if(!useSimulatedEnvironment) 
{
    var serialPortName = ArgsHelpers.GetOrDefault(args, 0, "/dev/ttyUSB0");
    var serialPortBaudRate = ArgsHelpers.GetOrDefault(args, 1, 115200);
    settings = PhilipsIntellivueClientSettings.CreateForPhysicalSerialPort(serialPortName, serialPortBaudRate, TimeSpan.FromSeconds(10), PollMode.Extended);
}
else 
{
    simulatedSerialPortPair = new SimulatedSerialPortPair();
    settings = PhilipsIntellivueClientSettings.CreateForSimulatedSerialPort(simulatedSerialPortPair.Port1, TimeSpan.FromSeconds(10), PollMode.Extended);

    // Simulated monitor
    simulatedMonitor = new SimulatedPhilipsIntellivueMonitor(simulatedSerialPortPair.Port2);
    simulatedMonitor.Start();
}

// Communicate
var philipsIntellivueCommunicator = new PhilipsIntellivueClient(settings);
philipsIntellivueCommunicator.NewMessage += StoreMessageAsJson;
philipsIntellivueCommunicator.Connect(
    TimeSpan.FromSeconds(1), 
    ExtendedPollProfileOptions.POLL_EXT_PERIOD_NU_1SEC | ExtendedPollProfileOptions.POLL_EXT_PERIOD_RTSA | ExtendedPollProfileOptions.POLL_EXT_ENUM);
philipsIntellivueCommunicator.StartPolling(); // TODO: Add polling settings (which values and waves to poll)
philipsIntellivueCommunicator.SendPatientDemographicsRequest();
while (Console.ReadKey(true).Key != ConsoleKey.Escape);
philipsIntellivueCommunicator.Disconnect();
if(useSimulatedEnvironment)
{
    simulatedMonitor!.Stop();
    simulatedSerialPortPair!.Dispose();
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