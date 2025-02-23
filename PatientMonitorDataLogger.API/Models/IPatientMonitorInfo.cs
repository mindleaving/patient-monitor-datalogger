using Newtonsoft.Json;
using PatientMonitorDataLogger.API.Models.Converters;

namespace PatientMonitorDataLogger.API.Models;

[JsonConverter(typeof(PatientMonitorInfoJsonConverter))]
public interface IPatientMonitorInfo
{
    PatientMonitorType Type { get; }
}