using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models.Converters;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Models;

[JsonConverter(typeof(MedicalDeviceInfoJsonConverter))]
public interface IMedicalDeviceInfo
{
    MedicalDeviceType DeviceType { get; }
}