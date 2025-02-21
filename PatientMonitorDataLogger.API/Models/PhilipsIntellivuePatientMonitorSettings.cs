using System.ComponentModel.DataAnnotations;

namespace PatientMonitorDataLogger.API.Models;

public class PhilipsIntellivuePatientMonitorSettings : IPatientMonitorSettings
{
    public PatientMonitorType Type => PatientMonitorType.PhilipsIntellivue;

    [Required]
    public string SerialPortName { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int SerialPortBaudRate { get; set; }
}