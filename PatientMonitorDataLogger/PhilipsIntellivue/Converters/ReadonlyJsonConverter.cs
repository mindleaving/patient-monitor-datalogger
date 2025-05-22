using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Converters;

public abstract class ReadonlyJsonConverter<T> : JsonConverter<T>
{
    public override bool CanWrite => false;

    public override void WriteJson(
        JsonWriter writer,
        T? value,
        JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }

    public override T? ReadJson(
        JsonReader reader,
        Type objectType,
        T? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var model = SelectModel(jObject);
        if (model == null)
            return default;
        serializer.Populate(jObject.CreateReader(), model);
        return model;
    }

    protected abstract T? SelectModel(
        JObject jObject);
}