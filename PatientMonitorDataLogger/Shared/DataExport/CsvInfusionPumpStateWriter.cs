using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.Shared.DataExport;

public class CsvInfusionPumpStateWriter : AsyncFileWriter<InfusionPumpState>, IInfusionPumpStateWriter
{
    private readonly char separator;

    public CsvInfusionPumpStateWriter(
        string outputFilePath,
        char separator = ';')
        : base(outputFilePath)
    {
        this.separator = separator;
    }

    protected override IEnumerable<string> Serialize(
        InfusionPumpState data)
    {
        foreach (var parameter in data.Parameters)
        {
            yield return $"{data.Timestamp:yyyy-MM-dd HH:mm:ss}{separator}{data.PumpIndex}{separator}{parameter.Name}{separator}{parameter.Value}";
        }
    }
}