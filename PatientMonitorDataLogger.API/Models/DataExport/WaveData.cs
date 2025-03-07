using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.API.Models.DataExport;

public class WaveData : IMonitorData
{
    public WaveData(
        string measurementType,
        DateTime timestampFirstDataPoint,
        double sampleRate,
        IList<float> values)
    {
        MeasurementType = measurementType;
        TimestampFirstDataPoint = timestampFirstDataPoint;
        SampleRate = sampleRate;
        Values = values;
    }

    public MonitorDataType Type => MonitorDataType.Wave;
    public string MeasurementType { get; }
    public DateTime TimestampFirstDataPoint { get; }
    /// <summary>
    /// Samples per second
    /// </summary>
    public double SampleRate { get; }
    public IList<float> Values { get; }
}