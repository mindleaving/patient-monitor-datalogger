using System.ComponentModel.DataAnnotations;

namespace PatientMonitorDataLogger.API.Models;

public class GEDashPatientMonitorSettings : PatientMonitorSettings
{
    public override PatientMonitorType Type => PatientMonitorType.GEDash;

    [Required]
    [RegularExpression("^(COM[0-9]+|/dev/tty[a-zA-Z0-9]+)$")]
    public string SerialPortName { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int SerialPortBaudRate { get; set; }
}