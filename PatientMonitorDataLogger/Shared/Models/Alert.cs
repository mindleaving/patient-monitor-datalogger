namespace PatientMonitorDataLogger.Shared.Models;

public class Alert
{
    public Alert() { }
    public Alert(
        DateTime timestamp,
        string parameterName,
        string text)
    {
        Timestamp = timestamp;
        ParameterName = parameterName;
        Text = text;
    }

    public DateTime Timestamp { get; set; }
    public string ParameterName { get; set; }
    public string Text { get; set; }
}