using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.GEDash.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.Shared.Models.Converters;

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
        if (!jObject.TryGetValue(nameof(PatientMonitorSettings.MonitorType), StringComparison.InvariantCultureIgnoreCase, out var typeString))
            throw new FormatException($"Missing or unexpected {nameof(PatientMonitorSettings.MonitorType)} field");
        if (!Enum.TryParse<PatientMonitorType>(typeString.Value<string>(), ignoreCase: true, out var monitorType))
            throw new FormatException($"Unknown monitor type {typeString}");
        PatientMonitorSettings monitorSettings = monitorType switch
        {
            PatientMonitorType.PhilipsIntellivue => new PhilipsIntellivuePatientMonitorSettings(),
            PatientMonitorType.SimulatedPhilipsIntellivue => new SimulatedPhilipsIntellivuePatientMonitorSettings(),
            PatientMonitorType.GEDash => new GEDashPatientMonitorSettings(),
            _ => throw new ArgumentOutOfRangeException(nameof(monitorType))
        };
        serializer.Populate(jObject.CreateReader(), monitorSettings);
        return monitorSettings;
    }
}