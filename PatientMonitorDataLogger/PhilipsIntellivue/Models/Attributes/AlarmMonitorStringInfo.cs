using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class AlarmMonitorStringInfo : AlarmMonitorGeneralInfo
{
    public AlarmMonitorStringInfo(
        ushort alarmInstanceNumber,
        TextId alarmText,
        ushort alertPriority,
        AlertFlags flags,
        IntellivueString str)
        : base(alarmInstanceNumber, alarmText, alertPriority, flags)
    {
        String = str;
    }

    public IntellivueString String { get; }

    public static AlarmMonitorStringInfo Read(
        BigEndianBinaryReader binaryReader)
    {
        var alarmInstanceNumber = binaryReader.ReadUInt16();
        var alarmText = TextId.Read(binaryReader);
        var alertPriority = binaryReader.ReadUInt16();
        var flags = (AlertFlags)binaryReader.ReadUInt16();
        var str = IntellivueString.Read(binaryReader);
        return new(
            alarmInstanceNumber,
            alarmText,
            alertPriority,
            flags,
            str);
    }

    public override byte[] Serialize()
    {
        return
        [
            ..base.Serialize(),
            ..String.Serialize()
        ];
    }
}