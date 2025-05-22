using Newtonsoft.Json;
using PatientMonitorDataLogger.PhilipsIntellivue.Converters;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

[JsonConverter(typeof(EventReportResultDataJsonConverter))]
public interface IEventReportResultData : ISerializable
{
}