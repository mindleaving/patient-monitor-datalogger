using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.Shared.DataExport;

public class CsvAlertWriter : AsyncFileWriter<Alert>, IAlertWriter
{
    private readonly char separator;

    public CsvAlertWriter(
        string outputFilePath,
        char separator = ';')
        : base(outputFilePath)
    {
        this.separator = separator;
    }

    protected override IEnumerable<string> Serialize(
        Alert data)
    {
        yield return $"{data.Timestamp:yyyy-MM-dd HH:mm:ss}{separator}{data.ParameterName}{separator}{data.Text}";
    }
}