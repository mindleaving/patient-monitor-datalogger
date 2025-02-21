using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class AlarmMonitorGeneralInfo : ISerializable
{
    public AlarmMonitorGeneralInfo(
        ushort alarmInstanceNumber,
        TextId alarmText,
        ushort alertPriority,
        AlertFlags flags)
    {
        AlarmInstanceNumber = alarmInstanceNumber;
        AlarmText = alarmText;
        AlertPriority = alertPriority;
        Flags = flags;
    }

    public ushort AlarmInstanceNumber { get; }
    public TextId AlarmText { get; }
    public ushort AlertPriority { get; }
    public AlertFlags Flags { get; }

    public static AlarmMonitorGeneralInfo Read(
        BigEndianBinaryReader binaryReader)
    {
        var alarmInstanceNumber = binaryReader.ReadUInt16();
        var alarmText = TextId.Read(binaryReader);
        var alertPriority = binaryReader.ReadUInt16();
        var flags = (AlertFlags)binaryReader.ReadUInt16();
        return new(
            alarmInstanceNumber,
            alarmText,
            alertPriority,
            flags);
    }

    public virtual byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(AlarmInstanceNumber),
            ..AlarmText.Serialize(),
            ..BigEndianBitConverter.GetBytes(AlertPriority),
            ..BigEndianBitConverter.GetBytes((ushort)Flags),
        ];
    }
}