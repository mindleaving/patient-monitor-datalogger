using System.Text.Json;
using System.Text.Json.Serialization;
using PatientMonitorDataLogger.API.Workflow;

namespace PatientMonitorDataLogger.API.Setups;

public class HubSetup : ISetup
{
    public void Run(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSignalR()
            .AddJsonProtocol(
                options =>
                {
                    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
        services.AddScoped<MeasurementDataDistributor>();
    }
}