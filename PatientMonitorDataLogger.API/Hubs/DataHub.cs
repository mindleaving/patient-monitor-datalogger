﻿using Microsoft.AspNetCore.SignalR;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.API.Models.DataExport;

namespace PatientMonitorDataLogger.API.Hubs;

public interface IDataHubClient
{
    Task ReceiveNumerics(
        Guid logSessionId,
        NumericsData data);

    Task ReceivePatientInfo(
        Guid logSessionId,
        PatientInfo patientInfo);

    Task ReceiveStatusChange(
        Guid logSessionId,
        LogStatus logStatus);
}

public class DataHub : Hub<IDataHubClient>
{
}