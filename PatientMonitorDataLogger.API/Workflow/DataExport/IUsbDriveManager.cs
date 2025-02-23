using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public interface IUsbDriveManager
{
    List<UsbDriveInfo> DiscoverUsbDrives();

    void CopyFolder(
        string sourcePath,
        string targetPath);
}