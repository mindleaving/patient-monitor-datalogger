using Microsoft.AspNetCore.SignalR;
using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.API.Hubs;

public interface IDataHubClient
{
    Task ReceiveNumerics(
        Guid logSessionId,
        NumericsData data);
}

public class DataHub : Hub<IDataHubClient>
{
}