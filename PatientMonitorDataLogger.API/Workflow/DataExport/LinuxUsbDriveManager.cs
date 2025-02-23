using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public class LinuxUsbDriveManager : IUsbDriveManager
{
    public List<UsbDriveInfo> DiscoverUsbDrives()
    {
        return DriveInfo.GetDrives()
            .Where(usbDrive => usbDrive.DriveType == DriveType.Removable && usbDrive.IsReady)
            .Select(x => new UsbDriveInfo(x.RootDirectory.FullName, TryGetLabel(x)))
            .ToList();
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

    private string? TryGetLabel(
        DriveInfo driveInfo)
    {
        try
        {
            if(driveInfo.IsReady)
                return driveInfo.VolumeLabel;
            return null;
        }
        catch
        {
            return null;
        }
    }
}