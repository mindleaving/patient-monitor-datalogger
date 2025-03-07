namespace PatientMonitorDataLogger.API.Models.DataExport;

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