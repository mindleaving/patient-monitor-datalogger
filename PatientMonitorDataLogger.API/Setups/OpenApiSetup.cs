namespace PatientMonitorDataLogger.API.Setups;

public class OpenApiSetup : ISetup
{
    public void Run(
        IServiceCollection services,
        IConfiguration configuration)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}