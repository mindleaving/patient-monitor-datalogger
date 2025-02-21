using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.DataExport;

public class LinuxUsbDriveManager : IUsbDriveManager
{
    public static List<UsbDriveInfo> DiscoverUsbDrives()
    {
        return [];
    }

    public void CopyFolder(
        string sourcePath,
        string targetPath)
    {
        if (!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);
        foreach (var sourceFilePath in Directory.EnumerateFiles(sourcePath))
        {
            var targetFilePath = Path.Combine(targetPath, Path.GetFileName(sourceFilePath));
            File.Copy(sourceFilePath, targetFilePath);
        }
    }
}