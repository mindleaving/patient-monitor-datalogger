using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PatientMonitorDataLogger.API.Setups;

public class ControllerSetup : ISetup
{
    public void Run(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers()
            .AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.Formatting = Formatting.None;
                });
    }
}