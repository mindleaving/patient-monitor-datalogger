using System.Text;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class CommandMessageReader
{
    public ICommandMessage Read(
        Stream stream)
    {
        var firstSessionHeaderByte = stream.ReadByte();
        if (firstSessionHeaderByte < 0)
            throw new EndOfStreamException();
        var isDataExportMessage = (byte)firstSessionHeaderByte == 0xE1;
        if (isDataExportMessage)
            return ReadDataExportCommandMessage(stream);
        return ReadAssociationCommandMessage(stream, (AssociationCommandType)firstSessionHeaderByte);
    }

    private ICommandMessage ReadAssociationCommandMessage(
        Stream stream,
        AssociationCommandType associationCommandType)
    {
        var length = LengthIndicator.Read(stream);
        var sessionHeader = new SessionHeader(associationCommandType, length);

        var binaryReader = new BigEndianBinaryReader(stream);
        var sessionData = Models.SessionData.Read(stream, sessionHeader);
        var presentationHeader = Models.PresentationHeader.Read(stream, sessionHeader);
        IAssociationCommandUserData? userData = sessionHeader.Type switch
        {
            AssociationCommandType.RequestAssociation => AssociationRequestUserData.Read(binaryReader),
            _ => null
        };
        var presentationTrailer = Models.PresentationTrailer.Read(stream, sessionHeader);
        return new AssociationCommandMessage(
            sessionHeader,
            sessionData,
            presentationHeader,
            userData,
            presentationTrailer);
    }

    private ICommandMessage ReadDataExportCommandMessage(
        Stream stream)
    {
        var secondSessionIdByte = stream.ReadByte();
        if (secondSessionIdByte < 0)
            throw new EndOfStreamException();
        var sessionId = BigEndianBitConverter.ToUInt16([0xE1, (byte)secondSessionIdByte]); // Expected to be 0xE100

        var binaryReader = new BigEndianBinaryReader(stream);
        var presentationContextId = binaryReader.ReadUInt16();
        var sessionPresentationHeader = new SessionPresentationHeader(presentationContextId);

        var remoteOperationType = (RemoteOperationType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var remoteOperationHeader = new RemoteOperationHeader(remoteOperationType, length);

        IRemoteOperationResult remoteOperationResult = remoteOperationHeader.Type switch
        {
            RemoteOperationType.Invoke => RemoteOperationInvoke.Read(binaryReader),
            RemoteOperationType.Result => RemoteOperationResult.Read(binaryReader),
            RemoteOperationType.Error => RemoteOperationError.Read(binaryReader),
            RemoteOperationType.LinkedResult => RemoteOperationLinkedResult.Read(binaryReader),
            _ => throw new ArgumentOutOfRangeException()
        };

        return new DataExportCommandMessage(
            sessionPresentationHeader,
            remoteOperationHeader,
            remoteOperationResult);
    }
}