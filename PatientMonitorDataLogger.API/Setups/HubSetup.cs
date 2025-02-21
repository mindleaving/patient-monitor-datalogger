using PatientMonitorDataLogger.API.Workflow;

namespace PatientMonitorDataLogger.API.Setups;

public class HubSetup : ISetup
{
    public void Run(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<MeasurementDataDistributor>();
    }
}