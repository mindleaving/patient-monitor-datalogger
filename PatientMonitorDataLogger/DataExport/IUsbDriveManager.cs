using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.DataExport;

public interface IUsbDriveManager
{
    static abstract List<UsbDriveInfo> DiscoverUsbDrives();

    void CopyFolder(
        string sourcePath,
        string targetPath);
}