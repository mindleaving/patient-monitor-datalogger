namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class AssociationCommandMessage : ICommandMessage
{
    public AssociationCommandMessage() { }

    public AssociationCommandMessage(
        SessionHeader sessionHeader,
        SessionData sessionData,
        PresentationHeader presentationHeader,
        IAssociationCommandUserData? userData,
        PresentationTrailer presentationTrailer)
    {
        SessionHeader = sessionHeader;
        SessionData = sessionData;
        PresentationHeader = presentationHeader;
        UserData = userData;
        PresentationTrailer = presentationTrailer;
    }

    public CommandMessageType MessageType => CommandMessageType.Association;

    public SessionHeader SessionHeader { get; set; }
    public SessionData SessionData { get; set; }
    public PresentationHeader PresentationHeader { get; set; }
    public IAssociationCommandUserData? UserData { get; set; }
    public PresentationTrailer PresentationTrailer { get; set; }

    public byte[] Serialize()
    {
        return
        [
            ..SessionHeader.Serialize(), 
            ..SessionData.Serialize(), 
            ..PresentationHeader.Serialize(), 
            ..UserData?.Serialize() ?? [],
            ..PresentationTrailer.Serialize()
        ];
    }

    public override string ToString()
    {
        return $"Association {SessionHeader.Type}";
    }
}