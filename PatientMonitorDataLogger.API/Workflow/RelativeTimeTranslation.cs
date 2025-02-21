using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.API.Workflow;

internal class RelativeTimeTranslation
{
    public RelativeTimeTranslation(
        DateTime absoluteTime,
        uint relativeTimeTicks)
    {
        AbsoluteTime = absoluteTime;
        RelativeTimeTicks = relativeTimeTicks;
    }

    public DateTime AbsoluteTime { get; }
    public uint RelativeTimeTicks { get; }

    public DateTime GetAbsoluteTime(
        uint ticks)
    {
        var microSecondsSinceReference = (ticks - RelativeTimeTicks) * 125;
        return AbsoluteTime.AddMicroseconds(microSecondsSinceReference);
    }
    public DateTime GetAbsoluteTime(
        RelativeTime relativeTime)
    {
        return GetAbsoluteTime(relativeTime.Ticks);
    }
}