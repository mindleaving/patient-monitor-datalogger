using System.IO.Ports;
using Microsoft.AspNetCore.Mvc;

namespace PatientMonitorDataLogger.API.Controllers;

public class SystemController : ApiController
{
    [HttpGet("serialports")]
    public async Task<IActionResult> GetAvailableSerialPorts()
    {
        return Ok(SerialPort.GetPortNames().OrderBy(x => x));
    }

}