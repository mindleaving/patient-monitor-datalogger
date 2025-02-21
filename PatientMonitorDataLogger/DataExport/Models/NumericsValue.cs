namespace PatientMonitorDataLogger.DataExport.Models;

public class NumericsValue
{
    public NumericsValue(
        double value,
        string? unit)
    {
        Value = value;
        Unit = unit;
    }

    public double Value { get; }
    public string? Unit { get; }
}