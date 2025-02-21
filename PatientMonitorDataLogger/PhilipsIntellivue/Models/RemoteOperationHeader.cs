using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RemoteOperationHeader : ISerializable
{
    public RemoteOperationHeader(
        RemoteOperationType type,
        ushort length)
    {
        Type = type;
        Length = length;
    }

    public RemoteOperationType Type { get; }
    public ushort Length { get; }

    public byte[] Serialize()
    {
        return [..BigEndianBitConverter.GetBytes((ushort)Type), ..BigEndianBitConverter.GetBytes(Length)];
    }
}