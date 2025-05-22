using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RemoteOperationHeader : ISerializable
{
    public RemoteOperationHeader() { }

    public RemoteOperationHeader(
        RemoteOperationType type,
        ushort length)
    {
        Type = type;
        Length = length;
    }

    public RemoteOperationType Type { get; set; }
    public ushort Length { get; set; }

    public byte[] Serialize()
    {
        return [..BigEndianBitConverter.GetBytes((ushort)Type), ..BigEndianBitConverter.GetBytes(Length)];
    }
}