using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class NumericObservedValue : ISerializable
{
    public NumericObservedValue()
    {
    }

    public NumericObservedValue(
        SCADAType physioId,
        MeasurementState state,
        UnitCodes unitCode,
        IntellivueFloat value)
    {
        PhysioId = physioId;
        State = state;
        UnitCode = unitCode;
        Value = value;
    }

    public SCADAType PhysioId { get; set; }
    public MeasurementState State { get; set; }
    public UnitCodes UnitCode { get; set; }
    public IntellivueFloat Value { get; set; }

    public static NumericObservedValue Read(
        BigEndianBinaryReader binaryReader)
    {
        var physioId = (SCADAType)binaryReader.ReadUInt16();
        var state = (MeasurementState)binaryReader.ReadUInt16();
        var unitCode = (UnitCodes)binaryReader.ReadUInt16();
        var value = IntellivueFloat.Read(binaryReader);
        return new(
            physioId,
            state,
            unitCode,
            value);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)PhysioId),
            ..BigEndianBitConverter.GetBytes((ushort)State),
            ..BigEndianBitConverter.GetBytes((ushort)UnitCode),
            ..Value.Serialize(),
        ];
    }
}