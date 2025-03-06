using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.Simulation;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class PhilipsIntellivueClientSettings
{
    private PhilipsIntellivueClientSettings(
        string serialPortName,
        int serialPortBaudRate,
        bool useSimulatedSerialPort,
        SimulatedIoDevice? simulatedSerialPort,
        TimeSpan messageRetentionPeriod,
        PollMode pollMode)
    {
        SerialPortName = serialPortName;
        SerialPortBaudRate = serialPortBaudRate;
        MessageRetentionPeriod = messageRetentionPeriod;
        UseSimulatedSerialPort = useSimulatedSerialPort;
        SimulatedSerialPort = simulatedSerialPort;
        PollMode = pollMode;
    }

    public static PhilipsIntellivueClientSettings CreateForPhysicalSerialPort(
        string serialPortname,
        int serialPortBaudRate,
        TimeSpan messageRetentionPeriod,
        PollMode pollMode)
        => new(serialPortname, serialPortBaudRate, false, null, messageRetentionPeriod, pollMode);

    public static PhilipsIntellivueClientSettings CreateForSimulatedSerialPort(
        SimulatedIoDevice simulatedSerialPort,
        TimeSpan messageRetentionPeriod,
        PollMode pollMode)
        => new(
            "COM0",
            115200,
            true,
            simulatedSerialPort,
            messageRetentionPeriod,
            pollMode);

    public string SerialPortName { get; }
    public int SerialPortBaudRate { get; }
    public bool UseSimulatedSerialPort { get; }
    public SimulatedIoDevice? SimulatedSerialPort { get; }
    public TimeSpan MessageRetentionPeriod { get; }
    public PollMode PollMode { get; set; }
}