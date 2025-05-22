namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class LinkedCommandMessageBundle
{
    public LinkedCommandMessageBundle(
        System.Collections.Generic.List<ICommandMessage> messages)
    {
        Messages = messages;
    }

    public System.Collections.Generic.List<ICommandMessage> Messages { get; }
}