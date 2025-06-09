using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.Shared.DataExport;

namespace PatientMonitorDataLogger.API.Workflow;

public abstract class PatientMonitorLogSessionRunner : LogSessionRunner
{
    protected new readonly LogSessionSettings logSessionSettings;
    protected readonly IAlertWriter alertWriter;
    protected readonly INumericsWriter numericsWriter;
    protected readonly Dictionary<string, IWaveWriter> waveWriters = new();

    protected PatientMonitorLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        DataWriterSettings writerSettings)
        : base(logSessionId, logSessionSettings, writerSettings)
    {
        this.logSessionSettings = logSessionSettings;
        var alertsOutputFilePath = Path.Combine(logSessionOutputDirectory, $"alerts_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv");
        alertWriter = new CsvAlertWriter(alertsOutputFilePath, logSessionSettings.CsvSeparator);
        var numericsOutputFilePath = Path.Combine(logSessionOutputDirectory, $"numerics_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv");
        numericsWriter = new CsvNumericsWriter(numericsOutputFilePath, logSessionSettings.CsvSeparator);
    }

    protected override void StartImpl()
    {
        alertWriter.Start();
        numericsWriter.Start();
        foreach (var waveWriter in waveWriters.Values)
        {
            waveWriter.Start();
        }
    }

    protected override void StopImpl()
    {
        alertWriter.Stop();
        numericsWriter.Stop();
        foreach(var waveWriter in waveWriters.Values)
        {
            waveWriter.Stop();
        }
    }

    protected IWaveWriter CreateWaveWriter(
        string measurementType)
    {
        var waveOutputFilePath = Path.Combine(logSessionOutputDirectory, $"{measurementType}_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv");
        IWaveWriter waveWriter = new CsvWaveWriter(measurementType, waveOutputFilePath, logSessionSettings.CsvSeparator);
        if (!waveWriters.TryAdd(measurementType, waveWriter))
        {
            waveWriter.Dispose(); // Dispose the wave writer we just created, and use the existing.
            waveWriter = waveWriters[measurementType];
        }
        else
        {
            waveWriter.Start();
        }
        return waveWriter;
    }
}