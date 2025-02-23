namespace PatientMonitorDataLogger.API.Models.DataExport;

public class NumericsValue
{
    public NumericsValue(
        DateTime timestamp,
        double value,
        string? unit)
    {
        Timestamp = timestamp;
        Value = value;
        Unit = unit;
    }

    public DateTime Timestamp { get; }
    public double Value { get; }
    public string? Unit { get; }
}