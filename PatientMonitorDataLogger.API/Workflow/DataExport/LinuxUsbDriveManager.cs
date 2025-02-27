using System.Text.RegularExpressions;
using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public class LinuxUsbDriveManager : IUsbDriveManager
{
    public List<UsbDriveInfo> DiscoverUsbDrives()
    {
        var mounts = File.ReadAllLines("/proc/mounts").Select(ParseMountEntry).Where(mount => mount != null).Cast<MountEntry>();
        return mounts
            .Where(mount => mount.DevicePath.StartsWith("/") && mount.MountPoint.StartsWith("/media/"))
            .Select(mount => new UsbDriveInfo(mount.MountPoint, mount.MountPoint))
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

    private MountEntry? ParseMountEntry(string line)
    {
        var splitted = Regex.Split(line, "\\s+");
        if (splitted.Length < 3)
            return null;
        var devicePath = splitted[0];
        var mountPoint = splitted[1];
        var options = splitted[2];
        return new MountEntry(devicePath, mountPoint, options);
    }

    private class MountEntry
    {
        public MountEntry(
            string devicePath,
            string mountPoint,
            string options)
        {
            DevicePath = devicePath;
            MountPoint = mountPoint;
            Options = options;
        }

        public string DevicePath { get; }
        public string MountPoint { get; }
        public string Options { get; }
    }
}