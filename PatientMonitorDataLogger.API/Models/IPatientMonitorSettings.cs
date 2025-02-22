using System.Text.Json.Serialization;
using PatientMonitorDataLogger.API.Models.Converters;

namespace PatientMonitorDataLogger.API.Models;

[JsonConverter(typeof(PatientMonitorSettingsJsonConverter))]
public interface IPatientMonitorSettings
{
    PatientMonitorType Type { get; }
}