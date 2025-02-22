using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models.Converters;

namespace PatientMonitorDataLogger.API.Models;

[JsonConverter(typeof(PatientMonitorSettingsJsonConverter))]
public abstract class PatientMonitorSettings
{
    public abstract PatientMonitorType Type { get; }
}