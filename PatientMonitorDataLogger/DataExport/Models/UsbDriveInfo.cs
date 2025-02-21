namespace PatientMonitorDataLogger.DataExport.Models;

public class UsbDriveInfo
{
    public UsbDriveInfo(
        string path,
        string name)
    {
        Path = path;
        Name = name;
    }

    public string Path { get; }
    public string Name { get; }
}