using Newtonsoft.Json;
using PatientMonitorDataLogger.Shared.Converters;

namespace PatientMonitorDataLogger.Shared.Models;

[JsonConverter(typeof(MedicalDeviceInfoJsonConverter))]
public interface IMedicalDeviceInfo
{
    MedicalDeviceType DeviceType { get; }
}