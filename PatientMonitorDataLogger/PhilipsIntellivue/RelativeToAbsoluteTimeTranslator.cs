using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class RelativeToAbsoluteTimeTranslator
{
    private readonly uint referenceTicks;
    private readonly DateTime referenceTime;

    public RelativeToAbsoluteTimeTranslator(
        uint referenceTicks,
        DateTime referenceTime)
    {
        this.referenceTicks = referenceTicks;
        this.referenceTime = referenceTime;
    }

    public DateTime Translate(
        uint ticks)
    {
        var delta = ticks - referenceTicks;
        return referenceTime.AddMicroseconds(delta * 125);
    }

    public RelativeTime GetCurrentRelativeTime()
    {
        var now = DateTime.UtcNow;
        var secondsSinceStart = (now - referenceTime).TotalSeconds;
        return new(TimeSpan.FromSeconds(secondsSinceStart));
    }
}