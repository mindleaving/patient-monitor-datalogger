using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Converters;

public class EventReportResultDataJsonConverter : ReadonlyJsonConverter<IEventReportResultData>
{
    protected override IEventReportResultData? SelectModel(
        JObject jObject)
    {
        return null;
    }
}