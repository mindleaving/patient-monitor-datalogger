using PatientMonitorDataLogger.BBraun.Helpers;

namespace PatientMonitorDataLogger.BBraun;

public class BBraunBccClientSettings
{
    public bool UseCharacterStuffing { get; set; }
}
public class BBraunBccClient
{
    private readonly BBraunBccClientSettings settings;
    private readonly BBraunBccFrameReader? frameReader;

    public BBraunBccClient(
        BBraunBccClientSettings settings)
    {
        this.settings = settings;
    }
}