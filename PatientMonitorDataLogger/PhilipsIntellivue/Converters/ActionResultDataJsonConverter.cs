using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Converters;

public class ActionResultDataJsonConverter : ReadonlyJsonConverter<IActionResultData>
{
    protected override IActionResultData? SelectModel(
        JObject jObject)
    {
        if (jObject.ContainsKey(nameof(ExtendedPollMdiDataReply.SequenceNumber)))
            return new ExtendedPollMdiDataReply();
        return new PollMdiDataReply();
    }
}