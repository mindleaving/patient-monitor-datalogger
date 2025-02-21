namespace PatientMonitorDataLogger.API.Setups;

public interface ISetup
{
    void Run(
        IServiceCollection services,
        IConfiguration configuration);
}