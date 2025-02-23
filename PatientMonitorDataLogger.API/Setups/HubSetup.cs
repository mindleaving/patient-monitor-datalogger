using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PatientMonitorDataLogger.API.Workflow;

namespace PatientMonitorDataLogger.API.Setups;

public class HubSetup : ISetup
{
    public void Run(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSignalR()
            .AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
                options.PayloadSerializerSettings.Formatting = Formatting.None;
            });
        services.AddSingleton<MeasurementDataDistributor>();
    }
}