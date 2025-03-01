using System.ComponentModel.DataAnnotations;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.API.Models;

public class LogSessionSettings
{
    public string Name { get; set; }
    [Required]
    public PatientMonitorSettings MonitorSettings { get; set; }
    [Required]
    public MonitorDataSettings MonitorDataSettings { get; set; }
    [Required]
    public char CsvSeparator { get; set; } = ';';
}