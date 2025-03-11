using Newtonsoft.Json;
using PatientMonitorDataLogger.Shared.Models.Converters;

namespace PatientMonitorDataLogger.Shared.Models;

[JsonConverter(typeof(MedicalDeviceSettingsJsonConverter))]
public interface IMedicalDeviceSettings
{
    MedicalDeviceType DeviceType { get; }
}