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
        NumericsData data)
    {
        await dataHub.Clients.All.ReceiveNumerics(data);
    }

    public async Task Distribute(
        PatientInfo data)
    {
        await dataHub.Clients.All.ReceivePatientInfo(data);
    }

    public async Task Distribute(
        LogStatus logStatus)
    {
        await dataHub.Clients.All.ReceiveStatusChange(logStatus);
    }
}