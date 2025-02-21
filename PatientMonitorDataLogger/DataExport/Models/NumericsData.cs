namespace PatientMonitorDataLogger.DataExport.Models;

public class NumericsData : IMonitorData
{
    public NumericsData(
        DateTime timestamp,
        Dictionary<MeasurementType, NumericsValue> values)
    {
        Timestamp = timestamp;
        Values = values;
    }

    public MonitorDataType Type => MonitorDataType.Numerics;
    public DateTime Timestamp { get; }
    public Dictionary<MeasurementType,NumericsValue> Values { get; }
}

public interface IMonitorData
{
    MonitorDataType Type { get; }
}

public enum MonitorDataType
{
    Undefined = 0, // For validation. Do not use.
    Numerics = 1,
    Wave = 2
}