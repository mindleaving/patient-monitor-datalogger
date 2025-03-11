using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest.PhilipsIntellivue;

public class MessageCreatorTest
{
    [Test]
    public void CanCreateAssociationRequest()
    {
        var sut = new CommandMessageCreator();
        var extendedPollProfile = new ExtendedPollProfile(ExtendedPollProfileOptions.None, new([]));
        var pollProfileSupport = new PollProfileSupport(
            PollProfileRevision.Revision0,
            new RelativeTime(TimeSpan.FromSeconds(1)),
            PollProfileSupport.MaxAllowedRs232Mtu,
            PollProfileSupport.MaxAllowedRs232Mtu,
            uint.MaxValue,
            PollProfileOptions.None,
            new(
            [
                new AttributeValueAssertion(
                    (ushort)ProtocolIdentification.NOM_ATTR_POLL_PROFILE_EXT,
                    extendedPollProfile)
            ]));
        var associationInfo = new MdseUserInfoStd(
            ProtocolVersion.Version1,
            NomenclatureVersion.Version,
            FunctionalUnits.None,
            SystemType.Client,
            StartupMode.ColdStart,
            new([]),
            new(
            [
                new AttributeValueAssertion(
                    (ushort)ProtocolIdentification.NOM_POLL_PROFILE_SUPPORT,
                    pollProfileSupport)
            ]));
        var associationRequestMessage = sut.CreateAssociationRequest(associationInfo);
        OutputAssociationMessage(associationRequestMessage);
    }

    [Test]
    public void CanCreateAssociationReleaseRequest()
    {
        var sut = new CommandMessageCreator();
        var associationReleaseRequest = sut.CreateAssociationReleaseRequest();
        OutputAssociationMessage(associationReleaseRequest);
    }

    [Test]
    public void CanCreateSinglePollRequest()
    {
        var sut = new CommandMessageCreator();
        ushort presentationContextId = 2;
        var singlePollRequest = sut.CreateSinglePollRequest(
            presentationContextId, 
            PollObjectTypes.Numerics,
            OIDType.NOM_ATTR_GRP_METRIC_VAL_OBS);
        OutputDataExportMessage(singlePollRequest);
    }

    private void OutputDataExportMessage(
        DataExportCommandMessage message)
    {
        Console.WriteLine("Session/presentation header:");
        WriteBinary(message.SessionPresentationHeader.Serialize(), 8);
        Console.WriteLine();
        Console.WriteLine("Remote operation header:");
        WriteBinary(message.RemoteOperationHeader.Serialize(), 8);
        Console.WriteLine();
        Console.WriteLine("Remote operation data:");
        WriteBinary(message.RemoteOperationData.Serialize(), 8);
    }

    private void OutputAssociationMessage(
        AssociationCommandMessage message)
    {
        Console.WriteLine("Session header:");
        WriteBinary(message.SessionHeader.Serialize(), columns: 8);
        Console.WriteLine();
        Console.WriteLine("Session data:");
        WriteBinary(message.SessionData.Serialize(), columns: 8);
        Console.WriteLine();
        Console.WriteLine("Presentation header:");
        WriteBinary(message.PresentationHeader.Serialize(), columns: 8);
        Console.WriteLine();
        Console.WriteLine("User data:");
        WriteBinary(message.UserData?.Serialize() ?? [], columns: 8);
        Console.WriteLine();
        Console.WriteLine("Presentation trailer:");
        WriteBinary(message.PresentationTrailer.Serialize(), columns: 8);
    }

    private void WriteBinary(
        byte[] bytes,
        int columns)
    {
        if (bytes.Length == 0)
        {
            Console.WriteLine("No data");
            return;
        }
        foreach (var chunk in bytes.Chunk(columns))
        {
            Console.WriteLine(string.Join(' ', chunk.Select(b => $"0x{b:X2}")));
        }
    }
}