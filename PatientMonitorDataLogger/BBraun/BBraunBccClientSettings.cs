using PatientMonitorDataLogger.BBraun.Helpers;

namespace PatientMonitorDataLogger.BBraun;

public class BBraunBccClientSettings
{
    public BBraunBccClientSettings(
        BccParticipantRole role,
        string spaceStationIp,
        ushort spaceStationPort,
        bool useCharacterStuffing,
        TimeSpan messageRetentionPeriod,
        TimeSpan pollPeriod)
    {
        if (pollPeriod < TimeSpan.FromSeconds(5))
            throw new ArgumentOutOfRangeException(nameof(pollPeriod), "Poll period must be at 5 seconds or more");
        Role = role;
        SpaceStationIp = spaceStationIp;
        SpaceStationPort = spaceStationPort;
        UseCharacterStuffing = useCharacterStuffing;
        MessageRetentionPeriod = messageRetentionPeriod;
        PollPeriod = pollPeriod;
    }

    public BccParticipantRole Role { get; }
    public bool UseCharacterStuffing { get; }
    public string SpaceStationIp { get; }
    public ushort SpaceStationPort { get; }
    public TimeSpan MessageRetentionPeriod { get; }
    public TimeSpan PollPeriod { get; }
}