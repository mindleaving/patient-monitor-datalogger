namespace PatientMonitorDataLogger.API.Models.DataExport;

public class NumericsData : IMonitorData
{
    public NumericsData(
        Guid logSessionId,
        DateTime timestamp,
        Dictionary<MeasurementType, NumericsValue> values)
    {
        LogSessionId = logSessionId;
        Timestamp = timestamp;
        Values = values;
    }

    public MonitorDataType Type => MonitorDataType.Numerics;
    public Guid LogSessionId { get; }
    public DateTime Timestamp { get; }
    public Dictionary<MeasurementType,NumericsValue> Values { get; }
}