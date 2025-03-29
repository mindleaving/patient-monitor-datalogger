import { MedicalDeviceType } from "../types/enums";
import { Models } from "../types/models";

export const compareDeviceTypes = (a: Models.IMedicalDeviceSettings, b: Models.IMedicalDeviceSettings) => {
    const deviceTypeComparison = a.deviceType.localeCompare(b.deviceType);
    if(deviceTypeComparison !== 0) {
        return deviceTypeComparison;
    }
    switch(a.deviceType) {
        case MedicalDeviceType.PatientMonitor:
        {
            const patientMonitorSettingsA = a as Models.PatientMonitorSettings;
            const patientMonitorSettingsB = b as Models.PatientMonitorSettings;
            return patientMonitorSettingsA.monitorType.localeCompare(patientMonitorSettingsB.monitorType);
        }
        case MedicalDeviceType.InfusionPumps:
        {
            const infusionPumpSettingsA = a as Models.InfusionPumpSettings;
            const infusionPumpSettingsB = b as Models.InfusionPumpSettings;
            return infusionPumpSettingsA.infusionPumpType.localeCompare(infusionPumpSettingsB.infusionPumpType);
        }
        default:
            throw new Error(`Comparison of device type ${a.deviceType} not implemented`);
    }
}

export const compareLogSessions = (a: Models.LogSession, b: Models.LogSession) => {
    const deviceTypeComparison = compareDeviceTypes(a.settings.deviceSettings, b.settings.deviceSettings);
    if(deviceTypeComparison !== 0) {
        return deviceTypeComparison;
    }
    return a.id.localeCompare(b.id);
}