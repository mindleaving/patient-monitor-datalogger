using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class EnumObservationValue : ISerializable
{
    public EnumObservationValue() { }

    public EnumObservationValue(
        SCADAType physioId,
        MeasurementState state,
        EnumValue value)
    {
        PhysioId = physioId;
        State = state;
        Value = value;
    }

    public SCADAType PhysioId { get; set; }
    public MeasurementState State { get; set; }
    public EnumValue Value { get; set; }

    public static EnumObservationValue Read(
        BigEndianBinaryReader binaryReader)
    {
        var physioId = (SCADAType)binaryReader.ReadUInt16();
        var state = (MeasurementState)binaryReader.ReadUInt16();
        var value = EnumValue.Read(binaryReader);
        return new(physioId, state, value);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)PhysioId),
            ..BigEndianBitConverter.GetBytes((ushort)State),
            ..Value.Serialize()
        ];
    }
}