using System.Globalization;
using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public class CsvWaveWriter : AsyncFileWriter<WaveData>, IWaveWriter
{
    public string MeasurementType { get; }
    private readonly char separator;

    public CsvWaveWriter(
        string measurementType,
        string outputFilePath,
        char separator = ';')
        : base(outputFilePath)
    {
        MeasurementType = measurementType;
        this.separator = separator;
    }

    public void Write(
        WaveData data)
    {
        if (data.MeasurementType != MeasurementType)
            throw new InvalidOperationException("Cannot write wave data for different measurement type than this data writer was intended for");
        dataQueue.Add(data);
    }

    protected override IEnumerable<string> Serialize(
        WaveData data)
    {
        var sampleInterval = 1d / data.SampleRate;
        for (var sampleIndex = 0; sampleIndex < data.Values.Count; sampleIndex++)
        {
            var sampleValue = data.Values[sampleIndex];
            var t = data.TimestampFirstDataPoint + TimeSpan.FromSeconds(sampleInterval * sampleIndex);
            var formattedValue = sampleValue.ToString("F2", CultureInfo.InvariantCulture);
            yield return $"{t:yyyy-MM-dd HH:mm:ss.fff}{separator}{formattedValue}";
        }
    }
}