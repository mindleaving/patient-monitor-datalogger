using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class SimulatedPhilipsIntellivueLogSessionRunner : PhilipsIntellivueLogSessionRunner
{
    private readonly SimulatedCable cable = new();

    public SimulatedPhilipsIntellivueLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        DataWriterSettings writerSettings)
        : base(logSessionId, logSessionSettings, writerSettings)
    {
    }

    public SimulatedPhilipsIntellivueMonitor? SimulatedMonitor { get; private set; }

    protected override void InitializeImpl()
    {
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
        cable.Dispose();
    }
}