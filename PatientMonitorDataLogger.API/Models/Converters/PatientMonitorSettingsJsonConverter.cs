using System.Text.Json;
using System.Text.Json.Serialization;

namespace PatientMonitorDataLogger.API.Models.Converters;

public class PatientMonitorSettingsJsonConverter : JsonConverter<IPatientMonitorSettings>
{
    public override IPatientMonitorSettings? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var jObject = JsonDocument.ParseValue(ref reader).RootElement;
        if (!jObject.TryGetProperty(nameof(IPatientMonitorSettings.Type), out var typeProperty) 
            && !jObject.TryGetProperty(nameof(IPatientMonitorSettings.Type).ToLower(), out typeProperty))
        {
            throw new FormatException($"Could not find {nameof(IPatientMonitorSettings.Type)} property");
        }
        var typeString = typeProperty.GetString();
        if (!Enum.TryParse<PatientMonitorType>(typeString, ignoreCase: true, out var monitorType))
            throw new FormatException($"Unknown patient monitor settings type {typeString}");
        switch (monitorType)
        {
            case PatientMonitorType.Unknown:
                throw new NotSupportedException();
            case PatientMonitorType.PhilipsIntellivue:
                return JsonSerializer.Deserialize<PhilipsIntellivuePatientMonitorSettings>(jObject.GetRawText(), options);
            case PatientMonitorType.GEDash:
                return JsonSerializer.Deserialize<GEDashPatientMonitorSettings>(jObject.GetRawText(), options);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        IPatientMonitorSettings value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}