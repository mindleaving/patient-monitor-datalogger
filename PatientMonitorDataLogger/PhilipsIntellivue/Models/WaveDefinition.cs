namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class WaveDefinition
{
    public WaveDefinition(
        string description,
        Labels label,
        SCADAType observedValue,
        IList<UnitCodes> units)
    {
        Description = description;
        Label = label;
        ObservedValue = observedValue;
        Units = units;
    }

    public string Description { get; }
    public Labels Label { get; }
    public SCADAType ObservedValue { get; }
    public IList<UnitCodes> Units { get; }
}