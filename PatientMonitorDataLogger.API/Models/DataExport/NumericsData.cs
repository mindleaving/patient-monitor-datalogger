﻿using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Models.DataExport;

public class NumericsData : IMonitorData
{
    public NumericsData(
        DateTime timestamp,
        Dictionary<string, NumericsValue> values)
    {
        Timestamp = timestamp;
        Values = values;
    }

    public MonitorDataType Type => MonitorDataType.Numerics;
    public DateTime Timestamp { get; }
    public Dictionary<string,NumericsValue> Values { get; }
}