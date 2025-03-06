﻿using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.Shared.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun;

public class BBraunBccClient : IDisposable
{
    private readonly BBraunBccClientSettings settings;
    private IODevice? ioDevice;
    private BBraunBccCommunicator? protocolCommunicator;
    private readonly BBraunBccMessageCreator messageCreator;
    private readonly object startStopLock = new();
    private readonly Timer pollTimer;

    public BBraunBccClient(
        BBraunBccClientSettings settings)
    {
        this.settings = settings;
        messageCreator = new BBraunBccMessageCreator(settings);
        pollTimer = new Timer(
            Poll,
            null,
            Timeout.Infinite,
            Timeout.Infinite);
    }

    public bool IsConnected { get; private set; }
    public string BedId { get; private set; } = "1";

    public void Connect()
    {
        if(IsConnected)
            return;
        lock (startStopLock)
        {
            if(IsConnected)
                return;

            Initialize();
            ioDevice!.Open();
            protocolCommunicator!.Start();
            protocolCommunicator.Enqueue(messageCreator.CreateInitializeCommunicationRequest());
            BedId = WaitForBedId();

            IsConnected = true;
        }
    }

    private bool isInitialized;
    private void Initialize()
    {
        if(isInitialized)
            return;
        ioDevice = new PhysicalTcpClient(settings.SpaceStationIp!, settings.SpaceStationPort!.Value);
        protocolCommunicator = new BBraunBccCommunicator(ioDevice, settings, nameof(BBraunBccCommunicator));
        isInitialized = true;
    }

    private string WaitForBedId()
    {
        var waitRequest = new WaitForRequest<BBraunBccFrame>(
            Guid.NewGuid(),
            x =>
            {
                if (x.UserData is not BBraunBccResponse responseData)
                    return false;
                if (responseData.Quadruples.Count != 1)
                    return false;
                var quadruple = responseData.Quadruples[0];
                return quadruple.Parameter == "GNACK";
            });
        protocolCommunicator!.WaitForMessage(waitRequest);
        if (!waitRequest.WaitHandle.WaitOne(TimeSpan.FromSeconds(5)))
            throw new TimeoutException("Didn't receive connection acknowledgement");
        return waitRequest.MatchingItem!.BedId;
    }

    public void StartPolling()
    {
        pollTimer.Change(TimeSpan.Zero, settings.PollPeriod);
    }

    private void Poll(
        object? state)
    {
        protocolCommunicator!.Enqueue(messageCreator.CreateGetAllRequest(BedId));
    }

    public void StopPolling()
    {
        pollTimer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public void Disconnect()
    {
        if(!IsConnected)
            return;
        lock (startStopLock)
        {
            if(!IsConnected)
                return;

            IsConnected = false;
            protocolCommunicator?.Stop();
            ioDevice?.Close();
        }
    }

    public void Dispose()
    {
        Disconnect();
        protocolCommunicator?.Dispose();
        ioDevice?.Dispose();
    }
}