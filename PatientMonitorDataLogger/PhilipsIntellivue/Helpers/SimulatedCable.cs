using PatientMonitorDataLogger.Shared.Simulation;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class SimulatedCable : IDisposable
{
    private readonly Timer transferTimer;

    public SimulatedCable()
    {
        End1 = new SimulatedIoDevice();
        End2 = new SimulatedIoDevice();
        transferTimer = new Timer(
            Transfer,
            null,
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(100));
    }

    public SimulatedIoDevice End1 { get; }
    public SimulatedIoDevice End2 { get; }

    private void Transfer(
        object? state)
    {
        TransferBetweenPorts(End1, End2);
        TransferBetweenPorts(End2, End1);
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