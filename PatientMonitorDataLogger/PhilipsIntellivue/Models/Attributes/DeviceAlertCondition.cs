using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class DeviceAlertCondition : ISerializable
{
    public DeviceAlertCondition(
        AlertState deviceAlertState,
        ushort changeCounter,
        AlertType maximumPhysiologicalAlarm,
        AlertType maximumTechnicalAlarm,
        AlertType maximumAuditoryAlarm)
    {
        DeviceAlertState = deviceAlertState;
        ChangeCounter = changeCounter;
        MaximumPhysiologicalAlarm = maximumPhysiologicalAlarm;
        MaximumTechnicalAlarm = maximumTechnicalAlarm;
        MaximumAuditoryAlarm = maximumAuditoryAlarm;
    }

    public AlertState DeviceAlertState { get; }
    public ushort ChangeCounter { get; }
    public AlertType MaximumPhysiologicalAlarm { get; }
    public AlertType MaximumTechnicalAlarm { get; }
    public AlertType MaximumAuditoryAlarm { get; }

    public static DeviceAlertCondition Read(
        BigEndianBinaryReader binaryReader)
    {
        var deviceAlertState = (AlertState)binaryReader.ReadUInt16();
        var changeCounter = binaryReader.ReadUInt16();
        var maximumPhysiologicalAlarm = (AlertType)binaryReader.ReadUInt16();
        var maximumTechnicalAlarm = (AlertType)binaryReader.ReadUInt16();
        var maximumAuditoryAlarm = (AlertType)binaryReader.ReadUInt16();
        return new(
            deviceAlertState,
            changeCounter,
            maximumPhysiologicalAlarm,
            maximumTechnicalAlarm,
            maximumAuditoryAlarm);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)DeviceAlertState),
            ..BigEndianBitConverter.GetBytes(ChangeCounter),
            ..BigEndianBitConverter.GetBytes((ushort)MaximumPhysiologicalAlarm),
            ..BigEndianBitConverter.GetBytes((ushort)MaximumTechnicalAlarm),
            ..BigEndianBitConverter.GetBytes((ushort)MaximumAuditoryAlarm),
        ];
    }
}