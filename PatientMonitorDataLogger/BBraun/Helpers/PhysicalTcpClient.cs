using System.Net.Sockets;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class PhysicalTcpClient : IODevice
{
    private readonly string targetHostname;
    private readonly ushort targetPort;
    private TcpClient? tcpClient;
    private readonly object startStopLock = new();

    public PhysicalTcpClient(
        string targetHostname,
        ushort targetPort)
    {
        this.targetHostname = targetHostname;
        this.targetPort = targetPort;
    }

    public bool IsConnected => tcpClient != null && tcpClient.Connected;

    public void Open()
    {
        if(IsConnected)
            return;
        lock (startStopLock)
        {
            if(IsConnected)
                return;

            // Dispose existing TCP client
            tcpClient?.Dispose();

            // Create new TCP client
            tcpClient = new TcpClient();
            tcpClient.Connect(targetHostname, targetPort);
        }
    }

    public int Read(
        byte[] buffer,
        int offset,
        int count)
    {
        if (!IsConnected)
            throw new InvalidOperationException("TCP client not connected");
        var stream = tcpClient!.GetStream();
        if (!stream.DataAvailable)
            return 0;
        return stream.Read(buffer, offset, count);
    }

    public int Read(
        byte[] buffer)
    {
        return Read(buffer, 0, buffer.Length);
    }

    public void Write(
        byte[] buffer,
        int offset,
        int count)
    {
        if (!IsConnected)
            throw new InvalidOperationException("TCP client not connected");
        tcpClient!.GetStream().Write(buffer, offset, count);
    }

    public void Write(
        byte[] buffer)
    {
        Write(buffer, 0, buffer.Length);
    }

    public void Close()
    {
        lock (startStopLock)
        {
            tcpClient?.Close();
        }
    }

    public void Dispose()
    {
        tcpClient?.Dispose();
    }
}