using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ActionCommand : IRemoteOperationInvokeData
{
    public ActionCommand(
        ManagedObjectId managedObject,
        OIDType actionType,
        ushort length,
        IActionData data)
    {
        ManagedObject = managedObject;
        ActionType = actionType;
        Length = length;
        Data = data;
    }

    public ManagedObjectId ManagedObject { get; }
    public uint Scope { get; } = 0;
    public OIDType ActionType { get; }
    public ushort Length { get; }
    public IActionData Data { get; }

    public static ActionCommand Read(
        BigEndianBinaryReader binaryReader)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var scope = binaryReader.ReadUInt32();
        var actionType = (OIDType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var data = actionType switch
        {
            OIDType.NOM_ACT_POLL_MDIB_DATA_EXT => ExtendedPollMdiDataRequest.Read(binaryReader),
            OIDType.NOM_ACT_POLL_MDIB_DATA => PollMdiDataRequest.Read(binaryReader),
            _ => throw new ArgumentOutOfRangeException(nameof(actionType))
        };
        return new ActionCommand(managedObject, actionType, length, data);
    }

    public byte[] Serialize()
    {
        return
        [
            ..ManagedObject.Serialize(),
            ..BigEndianBitConverter.GetBytes(Scope),
            ..BigEndianBitConverter.GetBytes((ushort)ActionType),
            ..BigEndianBitConverter.GetBytes(Length),
            ..Data.Serialize()
        ];
    }
}