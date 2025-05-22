using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.Shared.Models;

public class NumericsValue
{
    public NumericsValue(
        DateTime timestamp,
        double value,
        string? unit,
        MeasurementState state)
    {
        Timestamp = timestamp;
        Value = value;
        Unit = unit;
        State = state;
    }

    public DateTime Timestamp { get; }
    public double Value { get; }
    public string? Unit { get; }
    public MeasurementState State { get; }
}