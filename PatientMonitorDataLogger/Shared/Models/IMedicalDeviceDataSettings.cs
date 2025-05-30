﻿using Newtonsoft.Json;
using PatientMonitorDataLogger.Shared.Converters;

namespace PatientMonitorDataLogger.Shared.Models;

[JsonConverter(typeof(MedicalDeviceDataSettingsJsonConverter))]
public interface IMedicalDeviceDataSettings
{
    MedicalDeviceType DeviceType { get; }
}