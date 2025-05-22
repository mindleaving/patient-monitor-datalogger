namespace PatientMonitorDataLogger.Shared.Models;

public class InfusionPumpParameter
{
    public InfusionPumpParameter(
        string name,
        string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public string Value { get; }
}