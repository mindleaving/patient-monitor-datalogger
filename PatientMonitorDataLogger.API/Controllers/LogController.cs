using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Workflow;
using PatientMonitorDataLogger.DataExport.Models;

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

    [HttpGet("{id}/latest")]
    public async Task<IActionResult> GetLatestMeasurements(
        [FromRoute] Guid id)
    {
        if (!logSessions.TryGet(id, out var logSession))
            return NotFound();

        var demoData = new NumericsData(
            DateTime.UtcNow,
            new()
            {
                { MeasurementType.HeartRateEcg, new(76, "1/min") },
                { MeasurementType.SpO2, new(99, "%") }
            });
        return Ok(demoData);
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartRecording(
        [FromBody] LogSessionSettings body)
    {
        var logSession = await logSessions.CreateNew(body, writerSettings.Value);
        await logSession.Start();
        return Ok(logSession);
    }

    [HttpPost("{id}/stop")]
    public async Task<IActionResult> StopRecording(
        [FromRoute] Guid id)
    {
        if (!logSessions.TryRemove(id, out var logSession))
            return NotFound();
        await logSession!.Stop();
        return Ok(logSession.Status);
    }


}