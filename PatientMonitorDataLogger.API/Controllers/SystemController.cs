using System.Diagnostics;
using System.IO.Ports;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PatientMonitorDataLogger.API.Workflow;
using PatientMonitorDataLogger.PhilipsIntellivue;

namespace PatientMonitorDataLogger.API.Controllers;

public class SystemController : ApiController
{
    [HttpGet("version")]
    public async Task<IActionResult> GetVersion()
    {
        var version = Assembly.GetAssembly(typeof(PhilipsIntellivueClient))!.GetName().Version!.ToString();
        return Ok($"\"{version}\"");
    }


    [HttpGet("serialports")]
    public async Task<IActionResult> GetAvailableSerialPorts()
    {
        return Ok(SerialPort.GetPortNames().OrderBy(x => x));
    }

    [HttpPost("shutdown")]
    [HttpPost("poweroff")]
    public async Task<IActionResult> Shutdown(
        [FromServices] LogSessions logSessions)
    {
        if (logSessions.All.Any(x => x.Status.IsRunning))
            return BadRequest("One or more log sessions is still running. Stop them, before shutting down.");
        var result = Process.Start("sudo poweroff");
        result.WaitForExit(TimeSpan.FromSeconds(3));
        if (!result.HasExited)
        {
            result.Kill();
            return StatusCode((int)HttpStatusCode.InternalServerError, "Command didn't finish within 3 seconds. Aborting!");
        }
        if (result.ExitCode != 0)
            return StatusCode((int)HttpStatusCode.InternalServerError, $"Could not shutdown (exit code {result.ExitCode})");
        return Ok();
    }


}