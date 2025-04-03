using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Workflow;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Controllers;

public class LogController : ApiController
{
    private readonly LogSessions logSessions;
    private readonly IOptions<DataWriterSettings> writerSettings;

    public LogController(
        LogSessions logSessions,
        IOptions<DataWriterSettings> writerSettings)
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
        return Ok(logSession!.LatestObservations);
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
        [FromRoute] Guid id,
        [FromQuery] bool permanently = false)
    {
        if (!logSessions.TryRemove(id, out var logSession)) 
            return Ok();
        logSession!.Stop();
        logSession.Dispose();
        if (permanently)
            logSession.DeletePermanently();
        return Ok();
    }

    [HttpPost("{id}/events")]
    public async Task<IActionResult> CreateEvent(
        [FromRoute] Guid id,
        [FromBody] LogSessionEvent body)
    {
        if (!logSessions.TryGet(id, out var logSession))
            return NotFound();
        if (!logSession.ShouldBeRunning)
            return BadRequest("Log session isn't running");
        body.Timestamp = DateTime.UtcNow;
        logSession.LogCustomEvent(body);
        return Ok();
    }

}