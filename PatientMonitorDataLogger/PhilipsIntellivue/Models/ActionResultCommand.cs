using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ActionResultCommand : IRemoteOperationResultData, IRemoteOperationErrorData
{
    public ActionResultCommand() { }
    public ActionResultCommand(
        ManagedObjectId managedObject,
        OIDType actionType,
        ushort length,
        IActionResultData data)
    {
        ManagedObject = managedObject;
        ActionType = actionType;
        Length = length;
        Data = data;
    }

    public ManagedObjectId ManagedObject { get; set; }
    public OIDType ActionType { get; set; }
    public ushort Length { get; set; }
    public IActionResultData Data { get; set; }

    public byte[] Serialize()
    {
        return
        [
            ..ManagedObject.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)ActionType),
            ..BigEndianBitConverter.GetBytes(Length),
            ..Data.Serialize()
        ];
    }

    public static ActionResultCommand Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var actionType = (OIDType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        IActionResultData data = actionType switch
        {
            OIDType.NOM_ACT_POLL_MDIB_DATA_EXT => ExtendedPollMdiDataReply.Read(binaryReader, context),
            OIDType.NOM_ACT_POLL_MDIB_DATA => PollMdiDataReply.Read(binaryReader, context),
            _ => throw new ArgumentOutOfRangeException(nameof(actionType))
        };
        return new(managedObject, actionType, length, data);
    }
}