import { PatientMonitorType } from "../types/enums";
import { Models } from "../types/models";

interface MonitorSettingsDescriptionProps {
    monitorSettings: Models.PatientMonitorSettings;
}

export const MonitorSettingsDescription = (props: MonitorSettingsDescriptionProps) => {

    const { monitorSettings } = props;

    switch(monitorSettings.type) {
        case PatientMonitorType.PhilipsIntellivue:
        { 
            const philipsIntellivueSettings = monitorSettings as Models.PhilipsIntellivuePatientMonitorSettings;
            return (<span>
                Serial port: {philipsIntellivueSettings.serialPortName} @ {philipsIntellivueSettings.serialPortBaudRate} bit/s
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
            const geDashSettings = monitorSettings as Models.GEDashPatientMonitorSettings;
            return (<span>
                Serial port: {geDashSettings.serialPortName} @ {geDashSettings.serialPortBaudRate} bit/s
            </span>);
        }
        default:
            return null;
    }

}