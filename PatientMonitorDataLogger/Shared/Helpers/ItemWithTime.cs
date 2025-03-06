namespace PatientMonitorDataLogger.Shared.Helpers;

internal class ItemWithTime<T>(T item, DateTime receivedTimeStamp)
{
    public T Item { get; } = item;
    public DateTime ReceivedTimeStamp { get; } = receivedTimeStamp;
}