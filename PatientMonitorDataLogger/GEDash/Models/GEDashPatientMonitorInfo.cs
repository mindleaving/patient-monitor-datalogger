﻿using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.GEDash.Models;

public class GEDashPatientMonitorInfo : IPatientMonitorInfo
{
    public MedicalDeviceType DeviceType => MedicalDeviceType.PatientMonitor;
    public PatientMonitorType MonitorType => PatientMonitorType.GEDash;
    public string Name { get; set; } = "(no name)";
}