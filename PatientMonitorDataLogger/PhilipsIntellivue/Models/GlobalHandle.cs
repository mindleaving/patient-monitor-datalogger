using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class GlobalHandle : ISerializable
{
    public GlobalHandle(
        ushort contextId,
        ushort handle)
    {
        ContextId = contextId;
        Handle = handle;
    }

    public ushort ContextId { get; }
    public ushort Handle { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(ContextId),
            ..BigEndianBitConverter.GetBytes(Handle)
        ];
    }

    public static GlobalHandle Read(
        BigEndianBinaryReader binaryReader)
    {
        var contextId = binaryReader.ReadUInt16();
        var handle = binaryReader.ReadUInt16();
        return new(contextId, handle);
    }
}