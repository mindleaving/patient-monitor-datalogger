namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class AttributeContext
{
    public AttributeContext(
        CommandMessageType messageType)
    {
        MessageType = messageType;
    }

    public CommandMessageType MessageType { get; }
}