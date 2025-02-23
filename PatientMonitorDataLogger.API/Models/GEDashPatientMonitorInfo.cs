namespace PatientMonitorDataLogger.API.Models;

public class GEDashPatientMonitorInfo : IPatientMonitorInfo
{
    public PatientMonitorType Type => PatientMonitorType.GEDash;
    public string Name { get; set; } = "(no name)";
}