using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class NoSuchActionError : IRemoteOperationErrorData
{
    public NoSuchActionError(
        ObjectClass objectClassId,
        OIDType actionType)
    {
        ObjectClassId = objectClassId;
        ActionType = actionType;
    }

    public ObjectClass ObjectClassId { get; }
    public OIDType ActionType { get; }

    public static NoSuchActionError Read(
        BigEndianBinaryReader binaryReader)
    {
        var objectClassId = (ObjectClass)binaryReader.ReadUInt16();
        var actionType = (OIDType)binaryReader.ReadUInt16();
        return new(objectClassId, actionType);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)ObjectClassId),
            ..BigEndianBitConverter.GetBytes((ushort)ActionType)
        ];
    }
}