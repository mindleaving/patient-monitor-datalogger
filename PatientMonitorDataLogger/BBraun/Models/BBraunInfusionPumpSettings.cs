using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Models;

public class BBraunInfusionPumpSettings : InfusionPumpSettings
{
    public override InfusionPumpType InfusionPumpType => InfusionPumpType.BBraunSpace;
    public string? Hostname { get; set; }
    public ushort? Port { get; set; }
    public bool UseCharacterStuffing { get; set; } = false;
    public TimeSpan PollPeriod { get; set; } = TimeSpan.FromSeconds(10);
}