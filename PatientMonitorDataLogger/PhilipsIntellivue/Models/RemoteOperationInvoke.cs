using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RemoteOperationInvoke : IRemoteOperation
{
    public RemoteOperationInvoke(
        ushort invokeId,
        DataExportCommandType commandType,
        ushort length,
        IRemoteOperationInvokeData data)
    {
        InvokeId = invokeId;
        CommandType = commandType;
        Length = length;
        Data = data;
    }

    public ushort InvokeId { get; }
    public DataExportCommandType CommandType { get; }
    public ushort Length { get; }
    public IRemoteOperationInvokeData Data { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(InvokeId),
            ..BigEndianBitConverter.GetBytes((ushort)CommandType),
            ..BigEndianBitConverter.GetBytes(Length),
            ..Data.Serialize()
        ];
    }

    public static RemoteOperationInvoke Read(
        BigEndianBinaryReader binaryReader)
    {
        var context = new AttributeContext(CommandMessageType.DataExport);
        var invokeId = binaryReader.ReadUInt16();
        var commandType = (DataExportCommandType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        IRemoteOperationInvokeData data = commandType switch
        {
            DataExportCommandType.EventReport => EventReportCommand.Read(binaryReader, context),
            DataExportCommandType.ConfirmedEventReport => EventReportCommand.Read(binaryReader, context),
            DataExportCommandType.Get => GetCommand.Read(binaryReader),
            DataExportCommandType.Set => SetCommand.Read(binaryReader, context),
            DataExportCommandType.ConfirmedSet => SetCommand.Read(binaryReader, context),
            DataExportCommandType.ConfirmedAction => ActionCommand.Read(binaryReader, context),
            _ => throw new ArgumentOutOfRangeException()
        };
        return new(invokeId, commandType, length, data);
    }
}