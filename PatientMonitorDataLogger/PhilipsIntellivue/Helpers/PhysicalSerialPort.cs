using System.IO.Ports;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class PhysicalSerialPort : SerialPort, IODevice
{
    public PhysicalSerialPort(
        string portName,
        int baudRate,
        Parity parity,
        int dataBits,
        StopBits stopBits)
        : base(portName, baudRate, parity, dataBits, stopBits)
    {
    }

    public int Read(
        byte[] buffer)
    {
        return Read(buffer, 0, buffer.Length);
    }

    public void Write(
        byte[] buffer)
    {
        Write(buffer, 0, buffer.Length);
    }
}