using System.Collections.Concurrent;
using System.Text;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class SimulatedSerialPort : ISerialPort
{
    private readonly ConcurrentQueue<byte> incomingData = new();
    public ConcurrentQueue<byte> OutgoingData { get; } = new();
    public int IncomingBytesCount => incomingData.Count;

    public Encoding Encoding { get; set; } = Encoding.Unicode;

    public bool IsOpen { get; private set; }
    public void Open()
    {
        IsOpen = true;
    }

    public int Read(
        byte[] buffer,
        int offset,
        int count)
    {
        for (int i = 0; i < count; i++)
        {
            var hasData = incomingData.TryDequeue(out var b);
            if (!hasData)
                return i;
            var bufferIndex = offset + i;
            buffer[bufferIndex] = b;
        }

        return count;
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
        if (buffer.Length < offset + count)
            throw new ArgumentOutOfRangeException(nameof(count), "Requested count exceeds buffer size");

        for (int i = 0; i < count; i++)
        {
            var bufferIndex = offset + i;
            var b = buffer[bufferIndex];
            OutgoingData.Enqueue(b);
        }
    }

    public void Write(
        byte[] buffer)
    {
        Write(buffer, 0, buffer.Length);
    }

    public void Close()
    {
        IsOpen = false;
    }

    public void Receive(
        IEnumerable<byte> data)
    {
        foreach (var b in data)
        {
            incomingData.Enqueue(b);
        }
    }
}