import { InfusionPumpType, MedicalDeviceType, PatientMonitorType } from "../types/enums";
import { Models } from "../types/models";

interface DeviceSettingsDescriptionProps {
    deviceSettings: Models.IMedicalDeviceSettings;
}

export const DeviceSettingsDescription = (props: DeviceSettingsDescriptionProps) => {

    const { deviceSettings } = props;

    switch(deviceSettings.deviceType) {
        case MedicalDeviceType.PatientMonitor:
        {
            const patientMonitorSettings = deviceSettings as Models.PatientMonitorSettings;
            switch(patientMonitorSettings.monitorType) {
                case PatientMonitorType.PhilipsIntellivue:
                { 
                    const philipsIntellivueSettings = patientMonitorSettings as Models.PhilipsIntellivueSettings;
                    return (<span>
                        Philips Intellivue: Serial port {philipsIntellivueSettings.serialPortName} @ {philipsIntellivueSettings.serialPortBaudRate} bit/s
                    </span>);
                }
                case PatientMonitorType.SimulatedPhilipsIntellivue:
                {
                    return (<span>
                        Simulated Philips Intellivue Monitor
                    </span>);
                }
                case PatientMonitorType.GEDash:
                {
                    const geDashSettings = patientMonitorSettings as Models.GEDashSettings;
                    return (<span>
                        GE Dash: Serial port {geDashSettings.serialPortName} @ {geDashSettings.serialPortBaudRate} bit/s
                    </span>);
                }
                default:
                    return null;
            }
        }
        case MedicalDeviceType.InfusionPumps:
        {
            const infusionPumpSettings = deviceSettings as Models.InfusionPumpSettings;
            switch(infusionPumpSettings.infusionPumpType) {
                case InfusionPumpType.BBraunSpace:
                {
                    const bbraunInfusionPumpSettings = infusionPumpSettings as Models.BBraunInfusionPumpSettings;
                    return (<span>
                        B. Braun Space Station: IP {bbraunInfusionPumpSettings.hostname}:{bbraunInfusionPumpSettings.port}
                    </span>);
                }
            }
            break;
        }
    }
    

}