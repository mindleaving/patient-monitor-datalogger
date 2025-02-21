namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class WaitForRequest<T>
{
    public WaitForRequest(
        Guid id,
        Func<T, bool> predicate)
    {
        Id = id;
        Predicate = predicate;
    }

    public Guid Id { get; }
    public Func<T,bool> Predicate { get; }
    public ManualResetEvent WaitHandle { get; } = new(false);
    public T? MatchingItem { get; set; }

    public void Complete(
        T matchingItem)
    {
        MatchingItem = matchingItem;
        WaitHandle.Set();
    }
}