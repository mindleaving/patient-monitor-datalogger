using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RemoteOperationError : IRemoteOperationResult
{
    public RemoteOperationError(
        ushort invokeId,
        RemoteOperationErrorType errorValue,
        ushort length,
        IRemoteOperationErrorData? data)
    {
        InvokeId = invokeId;
        ErrorValue = errorValue;
        Length = length;
        Data = data;
    }

    public ushort InvokeId { get; }
    public RemoteOperationErrorType ErrorValue { get; }
    public ushort Length { get; }
    public IRemoteOperationErrorData? Data { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(InvokeId),
            ..BigEndianBitConverter.GetBytes((ushort)ErrorValue),
            ..BigEndianBitConverter.GetBytes(Length),
            ..(Data?.Serialize() ?? [])
        ];
    }

    public static RemoteOperationError Read(
        BigEndianBinaryReader binaryReader)
    {
        var context = new AttributeContext(CommandMessageType.DataExport);
        var invokeId = binaryReader.ReadUInt16();
        var errorValue = (RemoteOperationErrorType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        IRemoteOperationErrorData? data = errorValue switch
        {
            RemoteOperationErrorType.NoSuchObjectClass => NoSuchObjectClassError.Read(binaryReader),
            RemoteOperationErrorType.NoSuchObjectInstance => NoSuchObjectInstanceError.Read(binaryReader),
            RemoteOperationErrorType.AccessDenied => null,
            RemoteOperationErrorType.GetListError => GetListError.Read(binaryReader),
            RemoteOperationErrorType.SetListError => SetListError.Read(binaryReader),
            RemoteOperationErrorType.NoSuchAction => NoSuchActionError.Read(binaryReader),
            RemoteOperationErrorType.ProcessingFailure => ProcessingFailure.Read(binaryReader),
            RemoteOperationErrorType.InvalidArgumentValue => ActionResultCommand.Read(binaryReader, context),
            RemoteOperationErrorType.InvalidScope => InvalidScopeError.Read(binaryReader),
            RemoteOperationErrorType.InvalidObjectInstance => InvalidObjectInstanceError.Read(binaryReader),
            _ => throw new ArgumentOutOfRangeException()
        };
        return new(invokeId, errorValue, length, data);
    }
}