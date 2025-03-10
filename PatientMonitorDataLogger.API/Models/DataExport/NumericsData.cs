﻿using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.API.Models.DataExport;

public class NumericsData : IMonitorData, ILogSessionData
{
    public NumericsData(
        Guid logSessionId,
        DateTime timestamp,
        Dictionary<string, NumericsValue> values)
    {
        LogSessionId = logSessionId;
        Timestamp = timestamp;
        Values = values;
    }

    public MonitorDataType Type => MonitorDataType.Numerics;
    public Guid LogSessionId { get; }
    public DateTime Timestamp { get; }
    public Dictionary<string,NumericsValue> Values { get; }
}