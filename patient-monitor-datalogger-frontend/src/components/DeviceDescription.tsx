import { deviceTypeNames, infusionPumpSystemNames, monitorNames } from "../helpers/Formatters";
import { MedicalDeviceType } from "../types/enums";
import { Models } from "../types/models";

interface DeviceDescriptionProps {
    deviceSettings: Models.IMedicalDeviceSettings;
}

export const DeviceDescription = (props: DeviceDescriptionProps) => {

    const { deviceSettings } = props;
    
    const deviceType = deviceSettings.deviceType;
    const formattedDeviceType = deviceTypeNames[deviceType] ?? deviceType;
    switch(deviceType) {
        case MedicalDeviceType.PatientMonitor:
        {
            const patientMonitorSettings = deviceSettings as Models.PatientMonitorSettings;
            const monitorType = patientMonitorSettings.monitorType;
            const formattedMonitorType = monitorNames[monitorType] ?? monitorType;
            return (<span>
                {formattedMonitorType}
            </span>);
        }
        case MedicalDeviceType.InfusionPumps:
        {
            const infusionPumpSettings = deviceSettings as Models.InfusionPumpSettings;
            const pumpType = infusionPumpSettings.infusionPumpType;
            const formattedPumpType = infusionPumpSystemNames[pumpType] ?? pumpType;
            return (<span>
                {formattedPumpType}
            </span>);
        }
    }

}