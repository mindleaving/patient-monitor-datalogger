using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RemoteOperationLinkedResult : RemoteOperationResult
{
    public RemoteOperationLinkedResult(
        RemoteOperationLinkedResultId linkedId,
        ushort invokeId,
        DataExportCommandType commandType,
        ushort length,
        IRemoteOperationResultData data)
        : base(invokeId, commandType, length, data)
    {
        LinkedId = linkedId;
    }

    public RemoteOperationLinkedResultId LinkedId { get; }

    public override byte[] Serialize()
    {
        return
        [
            ..LinkedId.Serialize(),
            ..base.Serialize()
        ];
    }

    public new static RemoteOperationLinkedResult Read(
        BigEndianBinaryReader binaryReader)
    {
        var linkedId = RemoteOperationLinkedResultId.Read(binaryReader);
        var invokeId = binaryReader.ReadUInt16();
        var commandType = (DataExportCommandType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var data = ReadResultData(binaryReader, commandType);
        return new(
            linkedId,
            invokeId,
            commandType,
            length,
            data);
    }
}