namespace PatientMonitorDataLogger.Shared.Models;

public class PatientMonitorDataSettings : IMedicalDeviceDataSettings
{
    public MedicalDeviceType DeviceType => MedicalDeviceType.PatientMonitor;

    public bool IncludeAlerts { get; set; }
    public bool IncludeNumerics { get; set; }
    public bool IncludeWaves { get; set; }
    public bool IncludePatientInfo { get; set; }

    public List<string> SelectedNumericsTypes { get; set; }
    public List<WaveType> SelectedWaveTypes { get; set; }
}