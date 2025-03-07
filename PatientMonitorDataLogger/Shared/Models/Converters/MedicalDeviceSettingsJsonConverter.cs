using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.BBraun.Models;
using PatientMonitorDataLogger.GEDash.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.Shared.Models.Converters;

public class MedicalDeviceSettingsJsonConverter : JsonConverter<IMedicalDeviceSettings>
{
    public override bool CanWrite => false;

    public override void WriteJson(
        JsonWriter writer,
        IMedicalDeviceSettings? value,
        JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }

    public override IMedicalDeviceSettings? ReadJson(
        JsonReader reader,
        Type objectType,
        IMedicalDeviceSettings? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        if (!jObject.TryGetValue(nameof(IMedicalDeviceSettings.DeviceType), StringComparison.InvariantCultureIgnoreCase, out var deviceTypeString))
            throw new FormatException($"Missing or unexpected {nameof(IMedicalDeviceSettings.DeviceType)} field");
        if (!Enum.TryParse<MedicalDeviceType>(deviceTypeString.Value<string>(), ignoreCase: true, out var deviceType))
            throw new FormatException($"Unknown monitor type {deviceTypeString}");
        IMedicalDeviceSettings deviceSettings;
        switch (deviceType)
        {
            case MedicalDeviceType.PatientMonitor:
            {
                if (!jObject.TryGetValue(nameof(PatientMonitorSettings.MonitorType), StringComparison.InvariantCultureIgnoreCase, out var monitorTypeString))
                    throw new FormatException($"Missing or unexpected {nameof(PatientMonitorSettings.MonitorType)} field");
                if (!Enum.TryParse<PatientMonitorType>(monitorTypeString.Value<string>(), ignoreCase: true, out var monitorType))
                    throw new FormatException($"Unknown monitor type {monitorTypeString}");
                deviceSettings = monitorType switch
                {
                    PatientMonitorType.PhilipsIntellivue => new PhilipsIntellivueSettings(),
                    PatientMonitorType.SimulatedPhilipsIntellivue => new SimulatedPhilipsIntellivueSettings(),
                    PatientMonitorType.GEDash => new GEDashSettings(),
                    _ => throw new ArgumentOutOfRangeException(nameof(monitorType))
                };
                break;
            }
            case MedicalDeviceType.InfusionPumps:
            {
                if (!jObject.TryGetValue(nameof(InfusionPumpSettings.InfusionPumpType), StringComparison.InvariantCultureIgnoreCase, out var infusionPumpTypeString))
                    throw new FormatException($"Missing or unexpected {nameof(InfusionPumpSettings.InfusionPumpType)} field");
                if (!Enum.TryParse<InfusionPumpType>(infusionPumpTypeString.Value<string>(), ignoreCase: true, out var infusionPumpType))
                    throw new FormatException($"Unknown monitor type {infusionPumpTypeString}");
                deviceSettings = infusionPumpType switch
                {
                    InfusionPumpType.BBraunSpace => new BBraunInfusionPumpSettings(),
                    _ => throw new ArgumentOutOfRangeException(nameof(infusionPumpType))
                };
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(deviceType));
        }
        serializer.Populate(jObject.CreateReader(), deviceSettings);
        return deviceSettings;
    }
}