using PatientMonitorDataLogger.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace PatientMonitorDataLogger.BBraun.Models;

public class SimulatedBBraunInfusionPumpSettings : InfusionPumpSettings
{
    public override InfusionPumpType InfusionPumpType => InfusionPumpType.SimulatedBBraunSpace;

    [Required]
    [RegularExpression("^[/-9A-z]{1,15}$")]
    public string BedId { get; set; }
    [Required]
    [Range(1,3)]
    public int PillarCount { get; set; }
    [Required]
    [Range(0,24)]
    public int PumpCount { get; set; }
    [Required]
    public TimeSpan PollPeriod { get; set; } = TimeSpan.FromSeconds(10);
}