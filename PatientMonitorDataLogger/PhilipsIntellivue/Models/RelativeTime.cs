using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RelativeTime : ISerializable
{
    public RelativeTime() { }

    public RelativeTime(
        uint ticks)
    {
        Ticks = ticks;
    }
    public RelativeTime(
        TimeSpan time)
    {
        Ticks = (uint)(time.TotalMilliseconds * 8);
    }

    public uint Ticks { get; set; } // 1 tick = 125 us

    public double TotalSeconds => Ticks / 8000d;

    public byte[] Serialize()
    {
        return BigEndianBitConverter.GetBytes(Ticks);
    }

    public static RelativeTime Read(
        BigEndianBinaryReader binaryReader)
    {
        var ticks = binaryReader.ReadUInt32();
        return new(ticks);
    }
}