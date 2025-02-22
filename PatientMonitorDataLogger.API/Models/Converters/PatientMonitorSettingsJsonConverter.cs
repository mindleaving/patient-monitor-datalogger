using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PatientMonitorDataLogger.API.Models.Converters;

public class PatientMonitorSettingsJsonConverter : JsonConverter<PatientMonitorSettings>
{
    public override bool CanWrite => false;

    public override void WriteJson(
        JsonWriter writer,
        PatientMonitorSettings? value,
        JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }

    public override PatientMonitorSettings? ReadJson(
        JsonReader reader,
        Type objectType,
        PatientMonitorSettings? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        if (!jObject.TryGetValue(nameof(PatientMonitorSettings.Type), StringComparison.InvariantCultureIgnoreCase, out var typeString))
            throw new FormatException($"Missing or unexpected {nameof(PatientMonitorSettings.Type)} field");
        if (!Enum.TryParse<PatientMonitorType>(typeString.Value<string>(), ignoreCase: true, out var monitorType))
            throw new FormatException($"Unknown monitor type {typeString}");
        var monitorSettings = monitorType switch
        {
            PatientMonitorType.PhilipsIntellivue => new PhilipsIntellivuePatientMonitorSettings(),
            PatientMonitorType.GEDash => new PhilipsIntellivuePatientMonitorSettings(),
            _ => throw new ArgumentOutOfRangeException(nameof(monitorType))
        };
        serializer.Populate(jObject.CreateReader(), monitorSettings);
        return monitorSettings;
    }
}