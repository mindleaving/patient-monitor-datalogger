using PatientMonitorDataLogger.Shared.Simulation;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class SimulatedSerialPortPair : IDisposable
{
    private readonly Timer transferTimer;

    public SimulatedSerialPortPair()
    {
        Port1 = new SimulatedIoDevice();
        Port2 = new SimulatedIoDevice();
        transferTimer = new Timer(
            Transfer,
            null,
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(100));
    }

    public SimulatedIoDevice Port1 { get; }
    public SimulatedIoDevice Port2 { get; }

    private void Transfer(
        object? state)
    {
        TransferBetweenPorts(Port1, Port2);
        TransferBetweenPorts(Port2, Port1);
    }

    private void TransferBetweenPorts(
        SimulatedIoDevice sender,
        SimulatedIoDevice receiver)
    {
        var bytesAvailable = sender.OutgoingData.Count;
        if(bytesAvailable == 0)
            return;
        var buffer = new byte[bytesAvailable];
        var bytesRead = 0;
        while (bytesRead < bytesAvailable && sender.OutgoingData.TryDequeue(out var b))
        {
            buffer[bytesRead] = b;
            bytesRead++;
        }
        receiver.Receive(buffer);
    }

    public void Dispose()
    {
        transferTimer.Dispose();
    }
}