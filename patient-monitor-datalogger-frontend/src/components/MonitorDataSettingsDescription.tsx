import { useMemo } from "react";
import { Models } from "../types/models";
import { waveTypeNames } from "../helpers/Formatters";

interface MonitorDataSettingsDescriptionProps {
    monitorDataSettings: Models.MonitorDataSettings;
}

export const MonitorDataSettingsDescription = (props: MonitorDataSettingsDescriptionProps) => {

    const { monitorDataSettings } = props;

    const activatedParameters = useMemo(() => {
        const parameters: string[] = [];
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
        if(parameters.length === 0) {
            parameters.push("Nothing");
        }
        return parameters;
    }, [ monitorDataSettings ]);

    return (<span>
        Recording: {activatedParameters.join(", ")}
    </span>);

}