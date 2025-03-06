using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class Handle : ISerializable
{
    public Handle(
        ushort id)
    {
        Id = id;
    }

    public ushort Id { get; }

    public static Handle Read(
        BigEndianBinaryReader binaryReader)
    {
        return new(binaryReader.ReadUInt16());
    }

    public byte[] Serialize()
    {
        return BigEndianBitConverter.GetBytes(Id);
    }
}