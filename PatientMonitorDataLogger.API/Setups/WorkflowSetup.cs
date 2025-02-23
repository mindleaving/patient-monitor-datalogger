using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Workflow;

namespace PatientMonitorDataLogger.API.Setups;

public class WorkflowSetup : ISetup
{
    public void Run(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MonitorDataWriterSettings>(configuration.GetSection(MonitorDataWriterSettings.AppSettingsSectionName));
        services.AddSingleton<LogSessions>();
        var logSessionSupervisor = new LogSessionSupervisor();
        logSessionSupervisor.Start();
        services.AddSingleton(logSessionSupervisor);
    }
}