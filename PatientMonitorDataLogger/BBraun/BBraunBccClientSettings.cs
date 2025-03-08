using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.Shared.Simulation;

namespace PatientMonitorDataLogger.BBraun;

public class BBraunBccClientSettings
{
    private BBraunBccClientSettings(
        BccParticipantRole role,
        string spaceStationIp,
        ushort spaceStationPort,
        bool useCharacterStuffing,
        TimeSpan messageRetentionPeriod,
        TimeSpan pollPeriod,
        SimulatedIoDevice? simulatedIoDevice)
    {
        if (pollPeriod < TimeSpan.FromSeconds(5))
            throw new ArgumentOutOfRangeException(nameof(pollPeriod), "Poll period must be at 5 seconds or more");
        Role = role;
        SpaceStationIp = spaceStationIp;
        SpaceStationPort = spaceStationPort;
        UseCharacterStuffing = useCharacterStuffing;
        MessageRetentionPeriod = messageRetentionPeriod;
        PollPeriod = pollPeriod;
        SimulatedIoDevice = simulatedIoDevice;
    }

    public static BBraunBccClientSettings CreateForPhysicalConnection(
        BccParticipantRole role,
        string spaceStationIp,
        ushort spaceStationPort,
        bool useCharacterStuffing,
        TimeSpan messageRetentionPeriod,
        TimeSpan pollPeriod)
        => new(
            role,
            spaceStationIp,
            spaceStationPort,
            useCharacterStuffing,
            messageRetentionPeriod,
            pollPeriod,
            null);

    public static BBraunBccClientSettings CreateForSimulatedConnection(
        BccParticipantRole role,
        bool useCharacterStuffing,
        TimeSpan messageRetentionPeriod,
        TimeSpan pollPeriod,
        SimulatedIoDevice simulatedIoDevice)
        => new(
            role,
            "192.168.100.41",
            4001,
            useCharacterStuffing,
            messageRetentionPeriod,
            pollPeriod,
            simulatedIoDevice);

    public BccParticipantRole Role { get; }
    public bool UseCharacterStuffing { get; }
    public string SpaceStationIp { get; }
    public ushort SpaceStationPort { get; }
    public TimeSpan MessageRetentionPeriod { get; }
    public TimeSpan PollPeriod { get; }
    public bool UseSimulatedIoDevice => SimulatedIoDevice != null;
    public SimulatedIoDevice? SimulatedIoDevice { get; }
}