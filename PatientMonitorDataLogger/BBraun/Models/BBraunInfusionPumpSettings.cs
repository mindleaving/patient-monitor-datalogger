using System.ComponentModel.DataAnnotations;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Models;

public class BBraunInfusionPumpSettings : InfusionPumpSettings
{
    public override InfusionPumpType InfusionPumpType => InfusionPumpType.BBraunSpace;
    [Required]
    public string Hostname { get; set; }
    [Required]
    public ushort Port { get; set; }
    public bool UseCharacterStuffing { get; set; } = false;
    [Required]
    public TimeSpan PollPeriod { get; set; } = TimeSpan.FromSeconds(10);
}