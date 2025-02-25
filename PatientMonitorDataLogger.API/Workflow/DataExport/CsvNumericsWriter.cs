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

    public void Write(
        NumericsData data)
    {
        dataQueue.Add(data);
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
                serializedValues.Add(string.Empty);
                columnCount++;
            }

            serializedValues[columnIndex] = value.Value.Value.ToString("F2", CultureInfo.InvariantCulture);
        }

        if (hasNewMeasurementTypes)
        {
            var measurementTypes = header.OrderBy(x => x.Value).Select(kvp => kvp.Key).ToList();
            yield return $"Time{separator}{string.Join(separator, measurementTypes)}";
        }
        yield return $"{data.Timestamp:yyyy-MM-dd HH:mm:ss}{separator}{string.Join(separator, serializedValues)}";
    }
}