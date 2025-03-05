using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class NomenclatureReference : ISerializable, IEquatable<NomenclatureReference>
{
    public NomenclatureReference(
        NomenclaturePartition partition,
        ushort code)
    {
        Partition = partition;
        Code = code;
    }

    public NomenclaturePartition Partition { get; }
    public ushort Code { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)Partition),
            ..BigEndianBitConverter.GetBytes(Code)
        ];
    }

    public static NomenclatureReference Read(
        BigEndianBinaryReader binaryReader)
    {
        var partition = (NomenclaturePartition)binaryReader.ReadUInt16();
        var code = binaryReader.ReadUInt16();
        return new(partition, code);
    }

    public override string ToString()
    {
        return $"{Partition}:{Code}";
    }

    public bool Equals(
        NomenclatureReference? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Partition == other.Partition && Code == other.Code;
    }

    public override bool Equals(
        object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((NomenclatureReference)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Partition, Code);
    }

    public static bool operator ==(
        NomenclatureReference? left,
        NomenclatureReference? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        NomenclatureReference? left,
        NomenclatureReference? right)
    {
        return !Equals(left, right);
    }
}