using System.ComponentModel.DataAnnotations;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PhilipsIntellivuePatientMonitorSettings : PatientMonitorSettings
{
    public override PatientMonitorType MonitorType => PatientMonitorType.PhilipsIntellivue;

    [Required]
    [RegularExpression("^(COM[0-9]+|/dev/tty[a-zA-Z0-9]+)$")]
    public string SerialPortName { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int SerialPortBaudRate { get; set; }
}