using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Workflow;

namespace PatientMonitorDataLogger.API.Controllers;

public class LogController : ApiController
{
    private readonly LogSessions logSessions;
    private readonly IOptions<MonitorDataWriterSettings> writerSettings;

    public LogController(
        LogSessions logSessions,
        IOptions<MonitorDataWriterSettings> writerSettings)
    {
        this.logSessions = logSessions;
        this.writerSettings = writerSettings;
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetLogSessions()
    {
        return Ok(logSessions.All);
    }

    [HttpGet("{id}/status")]
    public async Task<IActionResult> GetLoggingStatus(
        [FromRoute] Guid id)
    {
        if (!logSessions.TryGet(id, out var logSession))
            return NotFound();
        return Ok(logSession!.Status);
    }

    [HttpGet("{id}/patient")]
    public async Task<IActionResult> GetPatientInfo(
        [FromRoute] Guid id)
    {
        if (!logSessions.TryGet(id, out var logSession))
            return NotFound();
        if (logSession!.PatientInfo == null)
            return NoContent();
        return Ok(logSession.PatientInfo);
    }


    [HttpGet("{id}/latest")]
    public async Task<IActionResult> GetLatestMeasurements(
        [FromRoute] Guid id)
    {
        if (!logSessions.TryGet(id, out var logSession))
            return NotFound();
        return Ok(logSession!.LatestMeasurements);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewLogSession(
        [FromBody] LogSessionSettings body)
    {
        var logSession = await logSessions.CreateNew(body, writerSettings.Value);
        return Ok(logSession);
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartRecording(
        [FromRoute] Guid id)
    {
        if (!logSessions.TryGet(id, out var logSession))
            return NotFound();
        try
        {
            logSession!.Start();
            return Ok(logSession.Status);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    [HttpPost("{id}/stop")]
    public async Task<IActionResult> StopRecording(
        [FromRoute] Guid id)
    {
        if (!logSessions.TryGet(id, out var logSession))
            return NotFound();
        try
        {
            logSession!.Stop();
            return Ok(logSession.Status);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLogSession(
        [FromRoute] Guid id)
    {
        if (logSessions.TryRemove(id, out var logSession))
        {
            logSession!.Stop();
            await logSession.DisposeAsync();
        }
        return Ok();
    }


}