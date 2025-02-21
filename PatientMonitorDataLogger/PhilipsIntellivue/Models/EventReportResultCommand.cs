using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class EventReportResultCommand : IRemoteOperationResultData
{
    public EventReportResultCommand(
        ManagedObjectId managedObject,
        RelativeTime currentTime,
        OIDType eventType,
        ushort length,
        IEventReportResultData? data)
    {
        ManagedObject = managedObject;
        CurrentTime = currentTime;
        EventType = eventType;
        Length = length;
        Data = data;
    }

    public ManagedObjectId ManagedObject { get; }
    public RelativeTime CurrentTime { get; }
    public OIDType EventType { get; }
    public ushort Length { get; }
    public IEventReportResultData? Data { get; set; }

    public byte[] Serialize()
    {
        return [
            ..ManagedObject.Serialize(), 
            ..CurrentTime.Serialize(), 
            ..BigEndianBitConverter.GetBytes((ushort)EventType), 
            ..BigEndianBitConverter.GetBytes(Length),
            ..(Data?.Serialize() ?? [])
        ];
    }

    public static EventReportResultCommand Read(
        BigEndianBinaryReader binaryReader)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var currentTime = RelativeTime.Read(binaryReader);
        var eventType = (OIDType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        IEventReportResultData? data = eventType switch
        {
            OIDType.NOM_NOTI_MDS_CREAT => null,
            _ => throw new ArgumentOutOfRangeException(nameof(eventType))
        };
        return new(
            managedObject,
            currentTime,
            eventType,
            length,
            data);
    }
}