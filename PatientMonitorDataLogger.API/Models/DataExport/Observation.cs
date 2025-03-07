namespace PatientMonitorDataLogger.API.Models.DataExport;

public class Observation
{
    public Observation(
        DateTime timestamp,
        string parameterName,
        string value,
        string? unit)
    {
        Timestamp = timestamp;
        ParameterName = parameterName;
        Value = value;
        Unit = unit;
    }

    public DateTime Timestamp { get; }
    public string ParameterName { get; }
    public string Value { get; }
    public string? Unit { get; }
}