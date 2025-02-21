using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class EventReportCommand : IRemoteOperationInvokeData
{
    public EventReportCommand(
        ManagedObjectId managedObject,
        RelativeTime eventTime,
        OIDType eventType,
        ushort length,
        IEventReportData data)
    {
        ManagedObject = managedObject;
        EventTime = eventTime;
        EventType = eventType;
        Length = length;
        Data = data;
    }

    public ManagedObjectId ManagedObject { get; }
    public RelativeTime EventTime { get; }
    public OIDType EventType { get; }
    public ushort Length { get; }
    public IEventReportData Data { get; }

    public byte[] Serialize()
    {
        return [
            ..ManagedObject.Serialize(), 
            ..EventTime.Serialize(), 
            ..BigEndianBitConverter.GetBytes((ushort)EventType), 
            ..BigEndianBitConverter.GetBytes(Length),
            ..Data.Serialize()
        ];
    }

    public static EventReportCommand Read(
        BigEndianBinaryReader binaryReader)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var eventTime = RelativeTime.Read(binaryReader);
        var eventType = (OIDType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var data = eventType switch
        {
            OIDType.NOM_NOTI_MDS_CREAT => MdsCreateInfo.Read(binaryReader),
            _ => throw new ArgumentOutOfRangeException(nameof(eventType), $"Invalid event report type {eventType}")
        };
        return new(managedObject, eventTime, eventType, length, data);
    }
}