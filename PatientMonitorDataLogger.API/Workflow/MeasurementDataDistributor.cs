using Microsoft.AspNetCore.SignalR;
using PatientMonitorDataLogger.API.Hubs;
using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.API.Workflow;

public class MeasurementDataDistributor
{
    private readonly IHubContext<DataHub, IDataHubClient> dataHub;

    public MeasurementDataDistributor(
        IHubContext<DataHub, IDataHubClient> dataHub)
    {
        this.dataHub = dataHub;
    }

    public async Task Distribute(
        Guid logSessionId,
        NumericsData data)
    {
        await dataHub.Clients.All.ReceiveNumerics(logSessionId, data);
    }
}