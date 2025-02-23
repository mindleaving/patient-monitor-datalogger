using System.ComponentModel.DataAnnotations;

namespace PatientMonitorDataLogger.API.Models.RequestBodies;

public class CopyDataToUsbDriveRequest
{
    [Required]
    public Guid LogSessionId { get; set; }
    [Required]
    public string UsbDrivePath { get; set; }
}