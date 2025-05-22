using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.BBraun.Models;
using PatientMonitorDataLogger.GEDash.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.Shared.Converters;

public class MedicalDeviceInfoJsonConverter : JsonConverter<IMedicalDeviceInfo>
{
    public override bool CanWrite => false;

    public override void WriteJson(
        JsonWriter writer,
        IMedicalDeviceInfo? value,
        JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }

    public override IMedicalDeviceInfo? ReadJson(
        JsonReader reader,
        Type objectType,
        IMedicalDeviceInfo? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        if (!jObject.TryGetValue(nameof(IMedicalDeviceInfo.DeviceType), StringComparison.InvariantCultureIgnoreCase, out var deviceTypeString))
            throw new FormatException();
        if (!Enum.TryParse<MedicalDeviceType>(deviceTypeString.Value<string>(), out var deviceType))
            throw new FormatException();
        IMedicalDeviceInfo deviceInfo;
        switch (deviceType)
        {
            case MedicalDeviceType.PatientMonitor:
            {
                if (!jObject.TryGetValue(nameof(IPatientMonitorInfo.MonitorType), StringComparison.InvariantCultureIgnoreCase, out var typeString))
                    throw new FormatException();
                if (!Enum.TryParse<PatientMonitorType>(typeString.Value<string>(), out var monitorType))
                    throw new FormatException();
                deviceInfo = monitorType switch
                {
                    PatientMonitorType.PhilipsIntellivue => new PhilipsIntellivuePatientMonitorInfo(),
                    PatientMonitorType.GEDash => new GEDashPatientMonitorInfo(),
                    _ => throw new ArgumentOutOfRangeException(nameof(monitorType))
                };
                break;
            }
            case MedicalDeviceType.InfusionPumps:
            {
                if (!jObject.TryGetValue(nameof(IInfusionPumpInfo.InfusionPumpType), StringComparison.InvariantCultureIgnoreCase, out var typeString))
                    throw new FormatException();
                if (!Enum.TryParse<InfusionPumpType>(typeString.Value<string>(), out var pumpType))
                    throw new FormatException();
                deviceInfo = pumpType switch
                {
                    InfusionPumpType.BBraunSpace => new BBraunInfusionPumpInfo(),
                    _ => throw new ArgumentOutOfRangeException(nameof(pumpType))
                };
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(deviceType));
        }
        serializer.Populate(jObject.CreateReader(), deviceInfo);
        return deviceInfo;
    }
}