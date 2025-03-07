using System.Globalization;
using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public class CsvNumericsWriter : AsyncFileWriter<NumericsData>, INumericsWriter
{
    private readonly char separator;
    private readonly Dictionary<string, int> header = new();

    public CsvNumericsWriter(
        string outputFilePath,
        char separator = ';')
        : base(outputFilePath)
    {
        this.separator = separator;
    }

    protected override IEnumerable<string> Serialize(
        NumericsData data)
    {
        var columnCount = header.Count;
        var hasNewMeasurementTypes = false;
        var serializedValues = Enumerable.Range(0, columnCount).Select(_ => string.Empty).ToList();
        foreach (var value in data.Values)
        {
            if (!header.TryGetValue(value.Key, out var columnIndex))
            {
                hasNewMeasurementTypes = true;
                columnIndex = columnCount;
                header.Add(value.Key, columnIndex);
                header.Add(value.Key + "_STATE", columnIndex + 1);
                serializedValues.Add(string.Empty);
                serializedValues.Add(string.Empty);
                columnCount += 2;
            }

            serializedValues[columnIndex] = value.Value.Value.ToString("F2", CultureInfo.InvariantCulture);
            serializedValues[columnIndex + 1] = value.Value.State.ToString();
        }

        if (hasNewMeasurementTypes)
        {
            var measurementTypes = header.OrderBy(x => x.Value).Select(kvp => kvp.Key).ToList();
            yield return $"Time{separator}{string.Join(separator, measurementTypes)}";
        }
        yield return $"{data.Timestamp:yyyy-MM-dd HH:mm:ss}{separator}{string.Join(separator, serializedValues)}";
    }
}