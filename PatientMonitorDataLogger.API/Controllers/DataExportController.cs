using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.RequestBodies;
using PatientMonitorDataLogger.API.Workflow.DataExport;

namespace PatientMonitorDataLogger.API.Controllers;

public class DataExportController : ApiController
{
    private readonly IUsbDriveManager usbDriveManager;
    private readonly IOptions<MonitorDataWriterSettings> writerSettings;

    public DataExportController(
        IUsbDriveManager usbDriveManager,
        IOptions<MonitorDataWriterSettings> writerSettings)
    {
        this.usbDriveManager = usbDriveManager;
        this.writerSettings = writerSettings;
    }

    [HttpGet("usb-drives")]
    public async Task<IActionResult> GetAvailableUsbDrives()
    {
        return Ok(usbDriveManager.DiscoverUsbDrives());
    }

    [HttpPost("to-usb")]
    public async Task<IActionResult> CopyDataToUsbDrive(
        [FromBody] CopyDataToUsbDriveRequest body)
    {
        var verifiedUsbDrivePath = usbDriveManager.DiscoverUsbDrives().FirstOrDefault(x => x.Path == body.UsbDrivePath);
        if (verifiedUsbDrivePath == null)
            return BadRequest("USB-drive not found");
        var logSessionId = body.LogSessionId;
        var logSessionDataFolder = Path.Combine(writerSettings.Value.OutputDirectory, logSessionId.ToString());
        if (!Directory.Exists(logSessionDataFolder))
            return BadRequest($"No data available for log session {logSessionId}");
        var usbDriveFolder = Path.Combine(body.UsbDrivePath, "patient-monitor-datalogger", logSessionId.ToString());
        try
        {
            if (!Directory.Exists(usbDriveFolder))
                Directory.CreateDirectory(usbDriveFolder);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, $"Could not create data directory {usbDriveFolder} on USB-drive: {e.Message}");
        }

        try
        {
            usbDriveManager.CopyFolder(logSessionDataFolder, usbDriveFolder);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

}