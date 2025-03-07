﻿using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Workflow.DataExport;

namespace PatientMonitorDataLogger.API.Workflow;

public abstract class PatientMonitorLogSessionRunner : LogSessionRunner
{
    protected new readonly LogSessionSettings logSessionSettings;
    protected readonly INumericsWriter numericsWriter;
    protected readonly Dictionary<string, IWaveWriter> waveWriters = new();

    protected PatientMonitorLogSessionRunner(
        Guid logSessionId,
        LogSessionSettings logSessionSettings,
        DataWriterSettings writerSettings)
        : base(logSessionId, logSessionSettings, writerSettings)
    {
        this.logSessionSettings = logSessionSettings;
        var numericsOutputFilePath = Path.Combine(logSessionOutputDirectory, $"numerics_{DateTime.UtcNow:yyyy-MM-dd_HHmmss}.csv");
        numericsWriter = new CsvNumericsWriter(numericsOutputFilePath, logSessionSettings.CsvSeparator);
    }

    protected override void StartImpl()
    {
        numericsWriter.Start();
        foreach (var waveWriter in waveWriters.Values)
        {
            waveWriter.Start();
        }
    }

    protected override void StopImpl()
    {
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