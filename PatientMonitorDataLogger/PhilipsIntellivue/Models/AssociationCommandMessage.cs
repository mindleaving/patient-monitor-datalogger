namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class AssociationCommandMessage : ICommandMessage
{
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

    public SessionHeader SessionHeader { get; }
    public SessionData SessionData { get; }
    public PresentationHeader PresentationHeader { get; }
    public IAssociationCommandUserData? UserData { get; }
    public PresentationTrailer PresentationTrailer { get; }

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