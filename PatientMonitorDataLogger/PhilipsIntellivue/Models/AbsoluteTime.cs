using System.Globalization;
using System.IO;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class AbsoluteTime : ISerializable
{
    public static AbsoluteTime Invalid = new(0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff);

    public AbsoluteTime() { }

    public AbsoluteTime(
        byte century,
        byte year,
        byte month,
        byte day,
        byte hour,
        byte minute,
        byte second,
        byte milliseconds = 0x00)
    {
        Century = century;
        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
        Milliseconds = milliseconds;
    }

    public byte Century { get; set; }
    public byte Year { get; set; }
    public byte Month { get; set; }
    public byte Day { get; set; }
    public byte Hour { get; set; }
    public byte Minute { get; set; }
    public byte Second { get; set; }
    public byte Milliseconds { get; set; } // Not used

    public DateTime ToDateTime()
    {
        var dateString = $"{Century:x2}{Year:x2}-{Month:x2}-{Day:x2} {Hour:x2}:{Minute:x2}:{Second:x2}";
        return DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public DateOnly ToDateOnly()
    {
        var dateString = $"{Century:x2}{Year:x2}-{Month:x2}-{Day:x2}";
        return DateOnly.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public static AbsoluteTime Read(
        BigEndianBinaryReader binaryReader)
    {
        var buffer = new byte[8];
        var bytesRead = binaryReader.Read(buffer, 0, buffer.Length);
        if (bytesRead != buffer.Length)
            throw new EndOfStreamException($"Expected absolute time with 8 bytes, but could only read {bytesRead} bytes");
        return new(
            buffer[0],
            buffer[1],
            buffer[2],
            buffer[3],
            buffer[4],
            buffer[5],
            buffer[6],
            buffer[7]);
    }

    public byte[] Serialize()
    {
        return [Century, Year, Month, Day, Hour, Minute, Second, Milliseconds];
    }
}