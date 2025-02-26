using System.Collections;
using System.Collections.Concurrent;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class AwaitableTimeCappedCollection<T> : ICollection<T>, IDisposable, IAsyncDisposable
{
    private readonly TimeSpan expirationTime;
    private readonly ConcurrentDictionary<Guid,WaitForRequest<T>> waitRequests = new();
    private readonly ConcurrentQueue<WaitForRequest<T>> newWaitRequests = new();
    private readonly ConcurrentQueue<ItemWithTime<T>> items = new();
    private readonly ConcurrentQueue<T> newItems = new();
    private readonly Timer expirationTimer;
    private readonly Timer itemWaitRequestMatchingTimer;

    public AwaitableTimeCappedCollection(
        TimeSpan expirationTime)
    {
        this.expirationTime = expirationTime;
        expirationTimer = new Timer(
            RemoveExpiredItems,
            null,
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(1));
        itemWaitRequestMatchingTimer = new Timer(
            MatchItemsToWaitRequests,
            null,
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(10));
    }

    private void MatchItemsToWaitRequests(
        object? state)
    {
        if(newItems.IsEmpty && newWaitRequests.IsEmpty)
            return;
        while (newWaitRequests.TryDequeue(out var newWaitRequest))
        {
            foreach (var itemWithTime in items)
            {
                if (newWaitRequest.Predicate(itemWithTime.Item))
                {
                    CompleteWaitRequest(newWaitRequest, itemWithTime.Item);
                    break;
                }
            }
        }

        while (newItems.TryDequeue(out var newItem))
        {
            foreach (var waitRequest in waitRequests.Values)
            {
                if (waitRequest.Predicate(newItem)) 
                    CompleteWaitRequest(waitRequest, newItem);
            }
        }
    }

    private void CompleteWaitRequest(
        WaitForRequest<T> waitRequest,
        T item)
    {
        waitRequest.Complete(item);
        waitRequests.TryRemove(waitRequest.Id, out _);
    }

    private void RemoveExpiredItems(
        object? state)
    {
        var now = DateTime.UtcNow;
        while (items.Count > 0 && items.TryPeek(out var nextItem) && now - nextItem.ReceivedTimeStamp > expirationTime)
        {
            items.TryDequeue(out _);
        }
    }

    public void Add(
        T item)
    {
        items.Enqueue(new(item, DateTime.UtcNow));
        newItems.Enqueue(item);
    }

    public void WaitFor(
        WaitForRequest<T> waitRequest)
    {
        waitRequests.TryAdd(waitRequest.Id, waitRequest);
        newWaitRequests.Enqueue(waitRequest);
    }

    public void StopWaiting(
        Guid waitRequestId)
    {
        waitRequests.TryRemove(waitRequestId, out _);
    }

    public void Clear()
    {
        items.Clear();
    }

    public bool Contains(
        T item)
    {
        return items.Any(x => x.Item!.Equals(item));
    }

    public void CopyTo(
        T[] array,
        int arrayIndex)
    {
        throw new NotSupportedException();
    }

    public bool Remove(
        T item)
    {
        throw new NotSupportedException();
    }

    public int Count => items.Count;

    public bool IsReadOnly => false;

    public IEnumerator<T> GetEnumerator()
    {
        return items.Select(x => x.Item).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Dispose()
    {
        expirationTimer.Dispose();
        itemWaitRequestMatchingTimer.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await expirationTimer.DisposeAsync();
        await itemWaitRequestMatchingTimer.DisposeAsync();
    }
}
internal class ItemWithTime<T>(T item, DateTime receivedTimeStamp)
{
    public T Item { get; } = item;
    public DateTime ReceivedTimeStamp { get; } = receivedTimeStamp;
}