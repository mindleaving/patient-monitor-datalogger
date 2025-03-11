using System.ComponentModel.DataAnnotations;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.GEDash.Models;

public class GEDashSettings : PatientMonitorSettings
{
    public override PatientMonitorType MonitorType => PatientMonitorType.GEDash;

    [Required]
    [RegularExpression("^(COM[0-9]+|/dev/tty[a-zA-Z0-9]+)$")]
    public string SerialPortName { get; set; }
    [Required]
    [AllowedValues(9600, 19200, 115200)]
    public int SerialPortBaudRate { get; set; }
}