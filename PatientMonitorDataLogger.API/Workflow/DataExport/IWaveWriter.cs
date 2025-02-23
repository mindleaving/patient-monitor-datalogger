﻿using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public interface IWaveWriter : IDisposable, IAsyncDisposable
{
    void Start();
    void Stop();
    void Write(
        WaveData data);
}