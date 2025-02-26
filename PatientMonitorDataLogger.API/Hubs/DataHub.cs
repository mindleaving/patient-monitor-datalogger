using Microsoft.AspNetCore.SignalR;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Hubs;

public interface IDataHubClient
{
    Task ReceiveNumerics(
        NumericsData data);

    Task ReceivePatientInfo(
        PatientInfo patientInfo);

    Task ReceiveStatusChange(
        LogStatus logStatus);
}

public class DataHub : Hub<IDataHubClient>
{
}