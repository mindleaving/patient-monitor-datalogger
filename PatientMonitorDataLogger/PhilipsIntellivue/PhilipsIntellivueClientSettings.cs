using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class PhilipsIntellivueClientSettings
{
    private PhilipsIntellivueClientSettings(
        string serialPortName,
        int serialPortBaudRate,
        bool useSimulatedSerialPort,
        SimulatedSerialPort? simulatedSerialPort,
        TimeSpan messageRetentionPeriod)
    {
        SerialPortName = serialPortName;
        SerialPortBaudRate = serialPortBaudRate;
        MessageRetentionPeriod = messageRetentionPeriod;
        UseSimulatedSerialPort = useSimulatedSerialPort;
        SimulatedSerialPort = simulatedSerialPort;
    }

    public static PhilipsIntellivueClientSettings CreateForPhysicalSerialPort(
        string serialPortname,
        int serialPortBaudRate,
        TimeSpan messageRetentionPeriod)
        => new(serialPortname, serialPortBaudRate, false, null, messageRetentionPeriod);

    public static PhilipsIntellivueClientSettings CreateForSimulatedSerialPort(
        SimulatedSerialPort simulatedSerialPort,
        TimeSpan messageRetentionPeriod)
        => new(
            "COM0",
            115200,
            true,
            simulatedSerialPort,
            messageRetentionPeriod);

    public string SerialPortName { get; }
    public int SerialPortBaudRate { get; }
    public bool UseSimulatedSerialPort { get; }
    public SimulatedSerialPort? SimulatedSerialPort { get; }
    public TimeSpan MessageRetentionPeriod { get; }
    public PollMode PollMode { get; set; } = PollMode.Single;
}
public enum PollMode
{
    Single,
    Extended
}