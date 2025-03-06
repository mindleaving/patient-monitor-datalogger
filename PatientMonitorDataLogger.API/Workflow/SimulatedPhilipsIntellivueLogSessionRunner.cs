using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class SimulatedPhilipsIntellivueLogSessionRunner : PhilipsIntellivueLogSessionRunner
{
    private SimulatedCable? cable;

    public SimulatedPhilipsIntellivueLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        MonitorDataWriterSettings writerSettings)
        : base(logSessionId, logSessionSettings, writerSettings)
    {
    }

    public SimulatedPhilipsIntellivueMonitor? SimulatedMonitor { get; private set; }

    protected override void InitializeImpl()
    {
        cable = new SimulatedCable();
        var monitorClientSettings = PhilipsIntellivueClientSettings.CreateForSimulatedSerialPort(cable.End1, TimeSpan.FromSeconds(10), PollMode.Extended);
        monitorClient = new PhilipsIntellivueClient(monitorClientSettings);
        SimulatedMonitor = new SimulatedPhilipsIntellivueMonitor(cable.End2);

        base.InitializeImpl();
    }

    protected override void StartImpl()
    {
        if (SimulatedMonitor == null)
            throw new InvalidOperationException("Simulated monitor is not initialized");
        SimulatedMonitor.Start();
        base.StartImpl();
    }

    protected override void StopImpl()
    {
        base.StopImpl();
        SimulatedMonitor?.Stop();
    }

    public override void Dispose()
    {
        base.Dispose();
        SimulatedMonitor?.Dispose();
        cable?.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        SimulatedMonitor?.Dispose();
        cable?.Dispose();
    }
}