using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PollDataRequestPeriod : ISerializable
{
    public PollDataRequestPeriod(
        RelativeTime activePeriod)
    {
        ActivePeriod = activePeriod;
    }

    public RelativeTime ActivePeriod { get; }

    public byte[] Serialize()
    {
        return ActivePeriod.Serialize();
    }

    public static PollDataRequestPeriod Read(
        BigEndianBinaryReader binaryReader)
    {
        var activePeriod = RelativeTime.Read(binaryReader);
        return new(activePeriod);
    }
}