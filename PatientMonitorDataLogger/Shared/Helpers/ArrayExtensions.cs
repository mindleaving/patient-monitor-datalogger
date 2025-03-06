namespace PatientMonitorDataLogger.Shared.Helpers;

public static class ArrayExtensions
{
    public static IEnumerable<T[]> Split<T>(
        this IEnumerable<T> items,
        T splitValue)
    {
        var part = new List<T>();
        foreach (var item in items)
        {
            if (item?.Equals(splitValue) ?? false)
            {
                yield return part.ToArray();
                part = new List<T>();
                continue;
            }
            part.Add(item);
        }
        yield return part.ToArray();
    }
}