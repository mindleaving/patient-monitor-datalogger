using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.Simulation;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class PhilipsIntellivueClientSettings
{
    private PhilipsIntellivueClientSettings(
        string serialPortName,
        int serialPortBaudRate,
        SimulatedIoDevice? simulatedSerialPort,
        TimeSpan messageRetentionPeriod,
        PollMode pollMode)
    {
        SerialPortName = serialPortName;
        SerialPortBaudRate = serialPortBaudRate;
        MessageRetentionPeriod = messageRetentionPeriod;
        SimulatedSerialPort = simulatedSerialPort;
        PollMode = pollMode;
    }

    public static PhilipsIntellivueClientSettings CreateForPhysicalSerialPort(
        string serialPortname,
        int serialPortBaudRate,
        TimeSpan messageRetentionPeriod,
        PollMode pollMode)
        => new(serialPortname, serialPortBaudRate, null, messageRetentionPeriod, pollMode);

    public static PhilipsIntellivueClientSettings CreateForSimulatedSerialPort(
        SimulatedIoDevice simulatedSerialPort,
        TimeSpan messageRetentionPeriod,
        PollMode pollMode)
        => new(
            "COM0",
            115200,
            simulatedSerialPort,
            messageRetentionPeriod,
            pollMode);

    public string SerialPortName { get; }
    public int SerialPortBaudRate { get; }
    public bool UseSimulatedSerialPort => SimulatedSerialPort != null;
    public SimulatedIoDevice? SimulatedSerialPort { get; }
    public TimeSpan MessageRetentionPeriod { get; }
    public PollMode PollMode { get; }
}