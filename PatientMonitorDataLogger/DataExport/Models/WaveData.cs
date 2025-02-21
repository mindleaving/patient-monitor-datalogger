namespace PatientMonitorDataLogger.DataExport.Models;

public class WaveData: IMonitorData
{
    public WaveData(
        MeasurementType measurementType,
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
    public MeasurementType MeasurementType { get; }
    public DateTime TimestampFirstDataPoint { get; }
    /// <summary>
    /// Samples per second
    /// </summary>
    public double SampleRate { get; }
    public IList<float> Values { get; }
}