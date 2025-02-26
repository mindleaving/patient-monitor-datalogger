namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class NumericsGenerator
{
    private readonly float lowerLimit;
    private readonly float upperLimit;
    private float currentValue;
    private readonly Random rng = new();

    public NumericsGenerator(
        float lowerLimit,
        float upperLimit)
    {
        this.lowerLimit = lowerLimit;
        this.upperLimit = upperLimit;
    }

    public float GetValue()
    {
        var normalizedDelta = 2 * (rng.NextDouble() - 0.5);
        var delta = (float)(0.1 * normalizedDelta);
        currentValue = Math.Clamp(currentValue + delta, lowerLimit, upperLimit);
        return currentValue;
    }
}