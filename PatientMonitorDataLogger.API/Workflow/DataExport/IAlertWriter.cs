﻿using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public interface IAlertWriter : IDisposable
{
    void Start();
    void Stop();
    
    void Write(
        Alert data);
}