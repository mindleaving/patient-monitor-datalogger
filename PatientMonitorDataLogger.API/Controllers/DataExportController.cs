using Microsoft.AspNetCore.Mvc;
using PatientMonitorDataLogger.API.Models.RequestBodies;
using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.API.Controllers;

public class DataExportController : ApiController
{
    [HttpGet("usb-drives")]
    public async Task<IActionResult> GetAvailableUsbDrives()
    {
        return Ok(new List<UsbDriveInfo>());
    }

    [HttpPost("to-usb")]
    public async Task<IActionResult> CopyDataToUsbDrive(
        [FromBody] CopyDataToUsbDriveRequest body)
    {

        return Ok();
    }

}