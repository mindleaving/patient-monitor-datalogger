using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RemoteOperationLinkedResultId : ISerializable
{
    public RemoteOperationLinkedResultId(
        RemoteOperationLinkedResultState state,
        byte count)
    {
        State = state;
        Count = count;
    }

    public RemoteOperationLinkedResultState State { get; }
    public byte Count { get; }

    public byte[] Serialize()
    {
        return
        [
            (byte)State, Count
        ];
    }

    public static RemoteOperationLinkedResultId Read(
        BigEndianBinaryReader binaryReader)
    {
        var state = (RemoteOperationLinkedResultState)binaryReader.ReadByte();
        var count = binaryReader.ReadByte();
        return new(state, count);
    }
}