using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RemoteOperationResult : IRemoteOperation
{
    public RemoteOperationResult() { }
    public RemoteOperationResult(
        ushort invokeId,
        DataExportCommandType commandType,
        ushort length,
        IRemoteOperationResultData data)
    {
        InvokeId = invokeId;
        CommandType = commandType;
        Length = length;
        Data = data;
    }

    public ushort InvokeId { get; set; }
    public DataExportCommandType CommandType { get; set; }
    public ushort Length { get; set; }
    public IRemoteOperationResultData Data { get; set; }

    public virtual byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(InvokeId),
            ..BigEndianBitConverter.GetBytes((ushort)CommandType),
            ..BigEndianBitConverter.GetBytes(Length),
            ..Data.Serialize()
        ];
    }

    public static RemoteOperationResult Read(
        BigEndianBinaryReader binaryReader)
    {
        var invokeId = binaryReader.ReadUInt16();
        var commandType = (DataExportCommandType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var data = ReadResultData(binaryReader, commandType);
        return new(
            invokeId,
            commandType,
            length,
            data);
    }

    public static IRemoteOperationResultData ReadResultData(
        BigEndianBinaryReader binaryReader,
        DataExportCommandType commandType)
    {
        var context = new AttributeContext(CommandMessageType.DataExport);
        return commandType switch
        {
            DataExportCommandType.EventReport => EventReportResultCommand.Read(binaryReader),
            DataExportCommandType.ConfirmedEventReport => EventReportResultCommand.Read(binaryReader),
            DataExportCommandType.Get => GetResultCommand.Read(binaryReader, context),
            DataExportCommandType.Set => SetResultCommand.Read(binaryReader, context),
            DataExportCommandType.ConfirmedSet => SetResultCommand.Read(binaryReader, context),
            DataExportCommandType.ConfirmedAction => ActionResultCommand.Read(binaryReader, context),
            _ => throw new ArgumentOutOfRangeException(nameof(commandType))
        };
    }
}