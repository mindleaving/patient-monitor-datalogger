using System.Text;
using System.Text.RegularExpressions;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Models;

public class PumpIndex : ISerializable, IEquatable<PumpIndex>, IComparable<PumpIndex>
{
    public PumpIndex(
        int pillar,
        int slot)
    {
        Pillar = pillar;
        Slot = slot;
    }

    /// <summary>
    /// Pillar index 1-3, 0=general rack parameters
    /// </summary>
    public int Pillar { get; }
    /// <summary>
    /// Counted from bottom of pillar 1-9A-O, 0=general rack parameters
    /// </summary>
    public int Slot { get; }
    public char SlotCharacter => Slot <= 9 ? (char)(Slot + '0') : (char)('A' + (Slot - 10));

    public static PumpIndex Parse(
        string str)
    {
        if (!Regex.IsMatch(str, "^501[0-3][0-9A-O]$"))
            throw new FormatException("Invalid pump index");
        var pillar = int.Parse(str.Substring(3, 1));
        var slot = GetSlotIndexFromSlotCharacter(str[4]);
        return new(pillar, slot);
    }

    public byte[] Serialize()
    {
        return Encoding.ASCII.GetBytes($"501{Pillar:0}{SlotCharacter:0}");
    }

    public override string ToString()
    {
        return $"{Pillar}{SlotCharacter}";
    }

    public static int GetSlotIndexFromSlotCharacter(
        char slotCharacter)
    {
        return char.IsNumber(slotCharacter) ? int.Parse(slotCharacter + "") : 10 + (slotCharacter - 'A');
    }

    public bool Equals(
        PumpIndex? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Pillar == other.Pillar && Slot == other.Slot;
    }

    public override bool Equals(
        object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PumpIndex)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Pillar, Slot);
    }

    public int CompareTo(PumpIndex? other)
    {
        if(other == null)
            return 1;
        if(Pillar != other.Pillar)
            return Pillar.CompareTo(other.Pillar);
        return Slot.CompareTo(other.Slot); 
    }

    public static bool operator ==(
        PumpIndex? left,
        PumpIndex? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        PumpIndex? left,
        PumpIndex? right)
    {
        return !Equals(left, right);
    }
}