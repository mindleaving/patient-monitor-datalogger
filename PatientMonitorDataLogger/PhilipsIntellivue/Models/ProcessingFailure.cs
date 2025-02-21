using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ProcessingFailure : IRemoteOperationErrorData
{
    public ProcessingFailure(
        OIDType errorId,
        ushort length,
        byte[] additionalInformation)
    {
        ErrorId = errorId;
        Length = length;
        AdditionalInformation = additionalInformation;
    }

    public OIDType ErrorId { get; }
    public ushort Length { get; }
    public byte[] AdditionalInformation { get; }

    public static ProcessingFailure Read(
        BigEndianBinaryReader binaryReader)
    {
        var errorId = (OIDType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var additionalInformation = new byte[length];
        var bytesRead = binaryReader.Read(additionalInformation);
        if (bytesRead != length)
            throw new EndOfStreamException();
        return new(errorId, length, additionalInformation);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)ErrorId),
            ..BigEndianBitConverter.GetBytes(Length),
            ..AdditionalInformation
        ];
    }
}