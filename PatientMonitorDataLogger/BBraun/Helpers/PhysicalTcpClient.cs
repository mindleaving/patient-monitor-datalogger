﻿using System.Net.Sockets;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class PhysicalTcpClient : TcpClient, IODevice
{
    private readonly string targetHostname;
    private readonly ushort targetPort;

    public PhysicalTcpClient(
        string targetHostname,
        ushort targetPort)
    {
        this.targetHostname = targetHostname;
        this.targetPort = targetPort;
    }

    public void Open()
    {
        Connect(targetHostname, targetPort);
    }

    public int Read(
        byte[] buffer,
        int offset,
        int count)
    {
        return GetStream().Read(buffer, offset, count);
    }

    public int Read(
        byte[] buffer)
    {
        return GetStream().Read(buffer);
    }

    public void Write(
        byte[] buffer,
        int offset,
        int count)
    {
        GetStream().Write(buffer, offset, count);
    }

    public void Write(
        byte[] buffer)
    {
        GetStream().Write(buffer);
    }
}