using System.ComponentModel.DataAnnotations;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PhilipsIntellivueSettings : PatientMonitorSettings
{
    public override PatientMonitorType MonitorType => PatientMonitorType.PhilipsIntellivue;

    [Required]
    [RegularExpression("^(COM[0-9]+|/dev/tty[a-zA-Z0-9]+)$")]
    public string SerialPortName { get; set; }
    [Required]
    [AllowedValues(19200, 115200)]
    public int SerialPortBaudRate { get; set; }
}