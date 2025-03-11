using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class SampleArrayObservedValue : ISerializable
{
    public SampleArrayObservedValue(
        SCADAType physioId,
        MeasurementState state,
        ObservedValueArray values)
    {
        PhysioId = physioId;
        State = state;
        Values = values;
    }

    public SCADAType PhysioId { get; }
    public MeasurementState State { get; }
    public ObservedValueArray Values { get; }

    public static SampleArrayObservedValue Read(
        BigEndianBinaryReader binaryReader)
    {
        var physioId = (SCADAType)binaryReader.ReadUInt16();
        var state = (MeasurementState)binaryReader.ReadUInt16();
        var values = ObservedValueArray.Read(binaryReader);
        return new(physioId, state, values);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)PhysioId),
            ..BigEndianBitConverter.GetBytes((ushort)State),
            ..Values.Serialize()
        ];
    }
}