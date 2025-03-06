using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun;

public class BBraunBccClientSettings
{
    public BBraunBccClientSettings(
        BccParticipantRole role,
        bool useCharacterStuffing,
        TimeSpan messageRetentionPeriod)
    {
        Role = role;
        UseCharacterStuffing = useCharacterStuffing;
        MessageRetentionPeriod = messageRetentionPeriod;
    }

    public BccParticipantRole Role { get; }
    public bool UseCharacterStuffing { get; }
    public string? SpaceStationIp { get; set; }
    public ushort? SpaceStationPort { get; set; }
    public TimeSpan MessageRetentionPeriod { get; }
}
public class BBraunBccClient : IDisposable
{
    private readonly BBraunBccClientSettings settings;
    private IODevice? ioDevice;
    private BBraunBccCommunicator? protocolCommunicator;

    public BBraunBccClient(
        BBraunBccClientSettings settings)
    {
        this.settings = settings;
    }

    public void Connect()
    {
        Initialize();
    }

    private bool isInitialized;
    private void Initialize()
    {
        if(isInitialized)
            return;
        ioDevice = new PhysicalTcpClient(settings.SpaceStationIp!, settings.SpaceStationPort!.Value);
        protocolCommunicator = new BBraunBccCommunicator(ioDevice, settings, nameof(BBraunBccCommunicator));
        isInitialized = true;
    }

    public void Dispose()
    {
        protocolCommunicator?.Dispose();
        ioDevice?.Dispose();
    }
}