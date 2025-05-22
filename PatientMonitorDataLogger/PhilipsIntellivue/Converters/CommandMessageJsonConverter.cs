using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Converters;

public class CommandMessageJsonConverter : ReadonlyJsonConverter<ICommandMessage>
{
    protected override ICommandMessage? SelectModel(
        JObject jObject)
    {
        var messageType = jObject.Value<string>(nameof(ICommandMessage.MessageType));
        switch (messageType)
        {
            case nameof(CommandMessageType.Association):
                return new AssociationCommandMessage();
            case nameof(CommandMessageType.DataExport):
                return new DataExportCommandMessage();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}