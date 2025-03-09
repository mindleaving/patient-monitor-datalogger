using Microsoft.Extensions.Options;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Workflow;
using PatientMonitorDataLogger.API.Workflow.DataExport;

namespace PatientMonitorDataLogger.API.Setups;

public class WorkflowSetup : ISetup
{
    public void Run(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DataWriterSettings>(configuration.GetSection(DataWriterSettings.AppSettingsSectionName));

        SetupLogSessionSupervisor(services);
        SetupLogSessions(services);

        SetupUsbDriveManager(services);
    }

    private static void SetupLogSessions(
        IServiceCollection services)
    {
        services.AddSingleton(
            provider =>
            {
                var measurementDataDistributor = provider.GetRequiredService<MeasurementDataDistributor>();
                var logSessionSupervisor = provider.GetRequiredService<LogSessionSupervisor>();
                var logSessions = new LogSessions(measurementDataDistributor, logSessionSupervisor);
                var writerSettings = provider.GetRequiredService<IOptions<DataWriterSettings>>().Value;
                logSessions.LoadFromDisk(writerSettings);
                return logSessions;
            });
        services.AddHostedService(provider => provider.GetRequiredService<LogSessions>()); // Ensures that LogSessions is started, independent of any controller requesting it as dependency. Otherwise it will only start at first request.
    }

    private void SetupLogSessionSupervisor(
        IServiceCollection services)
    {
        var logSessionSupervisor = new LogSessionSupervisor();
        logSessionSupervisor.Start();
        services.AddSingleton(logSessionSupervisor);
    }

    private static void SetupUsbDriveManager(
        IServiceCollection services)
    {
        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Win32S:
            case PlatformID.Win32Windows:
            case PlatformID.Win32NT:
            case PlatformID.WinCE:
                services.AddSingleton<IUsbDriveManager, WindowsUsbDriveManager>();
                break;
            case PlatformID.Unix:
                services.AddSingleton<IUsbDriveManager, LinuxUsbDriveManager>();
                break;
            case PlatformID.Xbox:
            case PlatformID.MacOSX:
            case PlatformID.Other:
                throw new NotSupportedException("No USB-drive manager available for the current platform");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}