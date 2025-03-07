using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PatientMonitorDataLogger.Shared.Models.Converters;

public class MedicalDeviceDataSettingsJsonConverter : JsonConverter<IMedicalDeviceDataSettings>
{
    public override bool CanWrite => false;

    public override void WriteJson(
        JsonWriter writer,
        IMedicalDeviceDataSettings? value,
        JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }

    public override IMedicalDeviceDataSettings? ReadJson(
        JsonReader reader,
        Type objectType,
        IMedicalDeviceDataSettings? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        if (!jObject.TryGetValue(nameof(IMedicalDeviceDataSettings.DeviceType), StringComparison.InvariantCultureIgnoreCase, out var deviceTypeString))
            throw new FormatException($"Missing or unexpected {nameof(IMedicalDeviceDataSettings.DeviceType)} field");
        if (!Enum.TryParse<MedicalDeviceType>(deviceTypeString.Value<string>(), ignoreCase: true, out var deviceType))
            throw new FormatException($"Unknown monitor type {deviceTypeString}");
        IMedicalDeviceDataSettings dataSettings = deviceType switch
        {
            MedicalDeviceType.PatientMonitor => new PatientMonitorDataSettings(),
            MedicalDeviceType.InfusionPumps => new InfusionPumpDataSettings(),
            _ => throw new ArgumentOutOfRangeException(nameof(deviceType))
        };
        serializer.Populate(jObject.CreateReader(), dataSettings);
        return dataSettings;
    }
}