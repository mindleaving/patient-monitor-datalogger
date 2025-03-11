namespace PatientMonitorDataLogger.BBraun.Models;

public class RackParameters
{
    public int NumberOfConnectedPillars { get; set; }
    public string? LastScannedBarcode { get; set; }
    public BBraunRackConfiguration RackConfiguration { get; set; }
    public ulong SerialNumber { get; set; }
    public string SoftwareVersion { get; set; }
}