using System.Globalization;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.Shared.DataExport;

public class CsvNumericsWriter : AsyncFileWriter<NumericsData>, INumericsWriter
{
    private readonly char separator;
    private readonly Dictionary<string, int> header = new();
    private bool hasNewMeasurementTypes;

    public CsvNumericsWriter(
        string outputFilePath,
        char separator = ';',
        IList<SCADAType>? measurementTypes = null)
        : base(outputFilePath)
    {
        this.separator = separator;
        if (measurementTypes != null)
        {
            foreach (var measurementType in measurementTypes)
            {
                var columnIndex = header.Count;
                header.Add(measurementType.ToString(), columnIndex);
                header.Add(measurementType + "_STATE", columnIndex + 1);
            }

            hasNewMeasurementTypes = true;
        }
    }

    protected override IEnumerable<string> Serialize(
        NumericsData data)
    {
        var serializedValues = Enumerable.Range(0, header.Count).Select(_ => string.Empty).ToList();
        foreach (var value in data.Values)
        {
            if (!header.TryGetValue(value.Key, out var columnIndex))
            {
                hasNewMeasurementTypes = true;
                columnIndex = header.Count;
                header.Add(value.Key, columnIndex);
                header.Add(value.Key + "_STATE", columnIndex + 1);
                serializedValues.Add(string.Empty);
                serializedValues.Add(string.Empty);
            }

            serializedValues[columnIndex] = value.Value.Value.ToString("F2", CultureInfo.InvariantCulture);
            serializedValues[columnIndex + 1] = value.Value.State.ToString();
        }

        if (hasNewMeasurementTypes)
        {
            yield return SerializeHeader();
            hasNewMeasurementTypes = false;
        }
        yield return $"{data.Timestamp:yyyy-MM-dd HH:mm:ss}{separator}{string.Join(separator, serializedValues)}";
    }

    private string SerializeHeader()
    {
        var measurementTypes = header.OrderBy(x => x.Value).Select(kvp => kvp.Key);
        return $"Time{separator}{string.Join(separator, measurementTypes)}";
    }
}