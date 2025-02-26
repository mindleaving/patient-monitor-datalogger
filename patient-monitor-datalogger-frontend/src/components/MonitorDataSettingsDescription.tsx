import { useMemo } from "react";
import { Models } from "../types/models";

interface MonitorDataSettingsDescriptionProps {
    monitorDataSettings: Models.MonitorDataSettings;
}

export const MonitorDataSettingsDescription = (props: MonitorDataSettingsDescriptionProps) => {

    const { monitorDataSettings } = props;

    const activatedParameters = useMemo(() => {
        const parameters: string[] = [];
        if(monitorDataSettings.includeAlerts) {
            parameters.push("Alerts");
        }
        if(monitorDataSettings.includeNumerics) {
            parameters.push("Numerics");
        }
        if(monitorDataSettings.includeWaves) {
            parameters.push("Waves");
        }
        if(monitorDataSettings.includePatientInfo) {
            parameters.push("Patient Info");
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