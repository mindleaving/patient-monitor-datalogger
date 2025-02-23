namespace PatientMonitorDataLogger.API.Models;

public class PatientInfo
{
    public string? PatientId { get; set; }
    public string? EncounterId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Sex? Sex { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Comment { get; set; }
}