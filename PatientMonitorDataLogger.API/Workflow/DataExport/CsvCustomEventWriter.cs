using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public class CsvCustomEventWriter : AsyncFileWriter<LogSessionEvent>
{
    private readonly char separator;

    public CsvCustomEventWriter(
        string outputFilePath,
        char separator = ';')
        : base(outputFilePath)
    {
        this.separator = separator;
    }

    protected override IEnumerable<string> Serialize(
        LogSessionEvent data)
    {
        yield return $"{data.Timestamp:yyyy-MM-dd HH:mm:ss.fff}{separator}{data.Message}";
    }
}