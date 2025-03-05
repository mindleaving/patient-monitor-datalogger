using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class SystemSpecificationEntry : ISerializable
{
    public SystemSpecificationEntry(
        ushort componentCapabilityId,
        ushort length,
        ushort[] values)
    {
        ComponentCapabilityId = componentCapabilityId;
        Length = length;
        Values = values;
    }

    public ushort ComponentCapabilityId { get; }
    public ushort Length { get; }
    public ushort[] Values { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(ComponentCapabilityId),
            ..BigEndianBitConverter.GetBytes(Length),
            ..Values.SelectMany(BigEndianBitConverter.GetBytes)
        ];

    }
}