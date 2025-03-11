import { useMemo } from "react";
import { Models } from "../types/models";
import { waveTypeNames } from "../helpers/Formatters";
import { MedicalDeviceType } from "../types/enums";

interface DeviceDataSettingsDescriptionProps {
    deviceDataSettings: Models.IMedicalDeviceDataSettings;
}

export const DeviceDataSettingsDescription = (props: DeviceDataSettingsDescriptionProps) => {

    const { deviceDataSettings } = props;

    const activatedParameters = useMemo(() => {
        const parameters: string[] = [];
        switch(deviceDataSettings.deviceType) {
            case MedicalDeviceType.PatientMonitor:
            { 
                const monitorDataSettings = deviceDataSettings as Models.PatientMonitorDataSettings;
                if(monitorDataSettings.includePatientInfo) {
                    parameters.push("Patient Info");
                }
                if(monitorDataSettings.includeAlerts) {
                    parameters.push("Alerts");
                }
                if(monitorDataSettings.includeNumerics) {
                    parameters.push("Numerics");
                }
                if(monitorDataSettings.includeWaves) {
                    if(monitorDataSettings.selectedWaveTypes.length > 0) {
                        const formattedWaves = monitorDataSettings.selectedWaveTypes.map(waveType => waveTypeNames[waveType] ?? waveType).join(", ");
                        parameters.push(`Waves (${formattedWaves})`);
                    } else {
                        parameters.push("Waves (All)");
                    }
                }
                break; 
            }
            case MedicalDeviceType.InfusionPumps:
            {
                break;
            }
        }
        if(parameters.length === 0) {
            parameters.push("Nothing");
        }
        return parameters;
    }, [ deviceDataSettings ]);

    return (<span>
        Recording: {activatedParameters.join(", ")}
    </span>);

}