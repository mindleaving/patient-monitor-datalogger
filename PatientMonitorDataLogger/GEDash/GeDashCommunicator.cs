using System.IO.Ports;

namespace PatientMonitorDataLogger.GEDash;

public class GeDashCommunicator
{
    private readonly byte[] dataRequestMessage = Convert.FromBase64String("QAAAAAAAAAAAAAAAAMoAIwABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAAAA9Mw=");
    private readonly SerialPort serialPort = new("COM9", 9600, Parity.None, 8, StopBits.One);

    public void Start()
    {
        Console.WriteLine("Data request message: " + Convert.ToBase64String(dataRequestMessage));

        serialPort.Handshake = Handshake.None;
        serialPort.DtrEnable = true;
        serialPort.RtsEnable = true;
        serialPort.Open();
        serialPort.DataReceived += SerialPortDataReceived;
        var dataRequestTimer = new Timer(
            SerialPortRequestData,
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

        dataRequestTimer.Change(Timeout.Infinite, Timeout.Infinite); // Stop requesting data
        serialPort.Close();
    }

    void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        var serialPort = (SerialPort)sender;
        var buffer = new byte[512];
        var data = new List<byte>();
        while(serialPort.BytesToRead > 0)
        {
            var bytesRead = serialPort.Read(buffer, 0, buffer.Length);
            data.AddRange(buffer[..bytesRead]);
        }
        Console.WriteLine("Data received: " + Convert.ToHexString(data.ToArray()));
    }

    void SerialPortRequestData(
        object? state)
    {
        serialPort.Write(dataRequestMessage, 0, dataRequestMessage.Length);
    }
}