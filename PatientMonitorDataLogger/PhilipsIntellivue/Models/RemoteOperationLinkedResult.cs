using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RemoteOperationLinkedResult : IRemoteOperation
{
    public RemoteOperationLinkedResult(
        RemoteOperationLinkedResultId linkedId,
        ushort invokeId,
        DataExportCommandType commandType,
        ushort length,
        IRemoteOperationResultData data)
    {
        LinkedId = linkedId;
        InvokeId = invokeId;
        CommandType = commandType;
        Length = length;
        Data = data;
    }

    public RemoteOperationLinkedResultId LinkedId { get; }
    public ushort InvokeId { get; }
    public DataExportCommandType CommandType { get; }
    public ushort Length { get; }
    public IRemoteOperationResultData Data { get; }

    public byte[] Serialize()
    {
        return
        [
            ..LinkedId.Serialize(),
            ..BigEndianBitConverter.GetBytes(InvokeId),
            ..BigEndianBitConverter.GetBytes((ushort)CommandType),
            ..BigEndianBitConverter.GetBytes(Length),
            ..Data.Serialize()
        ];
    }

    public static RemoteOperationLinkedResult Read(
        BigEndianBinaryReader binaryReader)
    {
        var linkedId = RemoteOperationLinkedResultId.Read(binaryReader);
        var invokeId = binaryReader.ReadUInt16();
        var commandType = (DataExportCommandType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var data = RemoteOperationResult.ReadResultData(binaryReader, commandType);
        return new(
            linkedId,
            invokeId,
            commandType,
            length,
            data);
    }
}