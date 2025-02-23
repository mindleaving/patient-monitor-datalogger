using Microsoft.AspNetCore.SignalR;
using PatientMonitorDataLogger.API.Hubs;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;

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

    public async Task Distribute(
        Guid logSessionId,
        PatientInfo data)
    {
        await dataHub.Clients.All.ReceivePatientInfo(logSessionId, data);
    }
}