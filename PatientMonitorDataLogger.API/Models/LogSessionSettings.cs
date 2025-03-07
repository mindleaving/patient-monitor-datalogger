using System.ComponentModel.DataAnnotations;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Models;

public class LogSessionSettings
{
    public string Name { get; set; }
    public MedicalDeviceType DeviceType { get; set; }
    public IMedicalDeviceSettings DeviceSettings { get; set; }
    public IMedicalDeviceDataSettings DataSettings { get; set; }
    [Required]
    public char CsvSeparator { get; set; } = ';';
}