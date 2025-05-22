using Newtonsoft.Json;
using PatientMonitorDataLogger.PhilipsIntellivue.Converters;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

[JsonConverter(typeof(ActionResultDataJsonConverter))]
public interface IActionResultData : ISerializable
{
}