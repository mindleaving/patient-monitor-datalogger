namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class WaveGenerator
{
    private readonly ushort[] data;
    private int currentIndex = 0;

    public WaveGenerator(
        ushort[] data)
    {
        this.data = data;
    }

    public ushort GetSingle() => data[currentIndex++ % data.Length];

    public IEnumerable<ushort> GetMany(
        int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return data[currentIndex++ % data.Length];
        }
    }
}