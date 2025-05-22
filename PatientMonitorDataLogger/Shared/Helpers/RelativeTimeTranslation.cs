using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.Shared.Helpers;

public class RelativeTimeTranslation
{
    public RelativeTimeTranslation(
        DateTime absoluteTime,
        ulong relativeTimeTicks,
        ulong microsecondsPerTick)
    {
        AbsoluteTime = absoluteTime;
        RelativeTimeTicks = relativeTimeTicks;
        MicrosecondsPerTick = microsecondsPerTick;
    }

    public static RelativeTimeTranslation PhilipsIntellivue(
        DateTime absoluteTime,
        ulong relativeTimeTicks)
        => new(absoluteTime, relativeTimeTicks, 125);
    public static RelativeTimeTranslation BBraunBccProtocol(
        DateTime absoluteTime,
        ulong relativeTimeTicks)
        => new(absoluteTime, relativeTimeTicks, 1_000_000);

    public DateTime AbsoluteTime { get; }
    public ulong RelativeTimeTicks { get; }
    public ulong MicrosecondsPerTick { get; }

    public DateTime GetAbsoluteTime(
        ulong ticks)
    {
        var microSecondsSinceReference = ((long)ticks - (long)RelativeTimeTicks) * (long)MicrosecondsPerTick;
        return AbsoluteTime.AddMicroseconds(microSecondsSinceReference);
    }
    public DateTime GetAbsoluteTime(
        RelativeTime relativeTime)
    {
        return GetAbsoluteTime(relativeTime.Ticks);
    }
}