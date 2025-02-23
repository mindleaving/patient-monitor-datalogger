using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PatientMonitorDataLogger.API.Models.Converters;

public class PatientMonitorInfoJsonConverter : JsonConverter<IPatientMonitorInfo>
{
    public override bool CanWrite => false;

    public override void WriteJson(
        JsonWriter writer,
        IPatientMonitorInfo? value,
        JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }

    public override IPatientMonitorInfo? ReadJson(
        JsonReader reader,
        Type objectType,
        IPatientMonitorInfo? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        if (!jObject.TryGetValue(nameof(IPatientMonitorInfo.Type), StringComparison.InvariantCultureIgnoreCase, out var typeString))
            throw new FormatException();
        if (!Enum.TryParse<PatientMonitorType>(typeString.Value<string>(), out var monitorType))
            throw new FormatException();
        IPatientMonitorInfo monitorInfo = monitorType switch
        {
            PatientMonitorType.PhilipsIntellivue => new PhilipsIntellivuePatientMonitorInfo(),
            PatientMonitorType.GEDash => new GEDashPatientMonitorInfo(),
            _ => throw new ArgumentOutOfRangeException()
        };
        serializer.Populate(jObject.CreateReader(), monitorInfo);
        return monitorInfo;
    }
}