import { useEffect } from "react";
import { MedicalDeviceType, WaveType } from "../../types/enums";
import { Models } from "../../types/models";
import { PatientMonitorDataSettingsFormControls } from "./PatientMonitorDataSettingsFormControls";

interface DeviceDataSettingsFormControlsProps {
    deviceSettings: Models.IMedicalDeviceSettings;
    value: Models.IMedicalDeviceDataSettings
    onChange: (update: Update<Models.IMedicalDeviceDataSettings>) => void;
}

export const DeviceDataSettingsFormControls = (props: DeviceDataSettingsFormControlsProps) => {

    const { deviceSettings, value, onChange } = props;

    useEffect(() => {
        if(value.deviceType !== deviceSettings.deviceType) {
            switch(deviceSettings.deviceType) {
                case MedicalDeviceType.PatientMonitor:
                    onChange(_ => ({
                        deviceType: deviceSettings.deviceType,
                        includePatientInfo: true,
                        includeAlerts: false,
                        includeNumerics: true,
                        includeWaves: false,
                        selectedNumericsTypes: [],
                        selectedWaveTypes: [
                            WaveType.ArterialBloodPressure,
                            WaveType.Pleth,
                            WaveType.Respiration,
                            WaveType.EcgDefault
                        ]
                    } as Models.PatientMonitorDataSettings));
                    break;
                case MedicalDeviceType.InfusionPumps:
                    onChange(_ => ({
                        deviceType: deviceSettings.deviceType,
                    } as Models.InfusionPumpDataSettings))
                    break;
                default:
                    onChange(_ => ({
                        deviceType: deviceSettings.deviceType
                    }));
                    break;
            }
        }
    }, [ deviceSettings, value, onChange ]);

    switch(deviceSettings.deviceType) {
        case MedicalDeviceType.PatientMonitor:
        {
            return (<PatientMonitorDataSettingsFormControls 
                deviceSettings={deviceSettings as Models.PatientMonitorSettings}
                value={value as Models.PatientMonitorDataSettings}
                onChange={onChange}
            />);
        }
        case MedicalDeviceType.InfusionPumps:
        {
            return (<>
            </>);
        }
        default:
            throw new Error(`Unknown device type ${deviceSettings.deviceType}. Cannot create data settings form controls`);
    }

}