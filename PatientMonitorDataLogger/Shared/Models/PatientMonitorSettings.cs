using Newtonsoft.Json;
using PatientMonitorDataLogger.Shared.Models.Converters;

namespace PatientMonitorDataLogger.Shared.Models;

[JsonConverter(typeof(PatientMonitorSettingsJsonConverter))]
public abstract class PatientMonitorSettings : IPatientMonitorSettings
{
    public MedicalDeviceType DeviceType => MedicalDeviceType.PatientMonitor;
    public abstract PatientMonitorType MonitorType { get; }
}