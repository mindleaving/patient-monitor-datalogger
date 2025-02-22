using System.ComponentModel.DataAnnotations;

namespace PatientMonitorDataLogger.API.Models;

public class PhilipsIntellivuePatientMonitorSettings : PatientMonitorSettings
{
    public override PatientMonitorType Type => PatientMonitorType.PhilipsIntellivue;

    [Required]
    [RegularExpression("^(COM[0-9]+|/dev/tty[a-zA-Z0-9]+)$")]
    public string SerialPortName { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int SerialPortBaudRate { get; set; }
}