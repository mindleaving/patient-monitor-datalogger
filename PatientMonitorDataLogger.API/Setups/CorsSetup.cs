namespace PatientMonitorDataLogger.API.Setups;

public class CorsSetup : ISetup
{
    public void Run(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(
            options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .WithOrigins(configuration["CORS:Origins"].Split(','))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("content-disposition")
                            .AllowCredentials();
                    });
            });
    }
}