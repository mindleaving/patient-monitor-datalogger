import { ReactNode, useCallback } from "react";
import { MedicalDeviceType, PatientMonitorType } from "../types/enums";
import { Models } from "../types/models";
import { GEDashSettingsFormControls } from "./GEDashSettingsFormControls";
import { PhilipsIntellivueSettingsFormControls } from "./PhilipsIntellivueSettingsFormControls";
import { SimulatedPhilipsIntellivueSettingsFormControls } from "./SimulatedPhilipsIntellivueSettingsFormControls";
import { FormControl, FormGroup, FormLabel } from "react-bootstrap";
import { monitorNames } from "../helpers/Formatters";

interface PatientMonitorSettingsFormControlsProps {
    value: Models.PatientMonitorSettings;
    onChange: (update: Update<Models.IMedicalDeviceSettings>) => void;
}

export const PatientMonitorSettingsFormControls = (props: PatientMonitorSettingsFormControlsProps) => {

    const { value, onChange } = props;

    const setMonitorType = useCallback((monitorType: PatientMonitorType) => {
        if(monitorType === value.monitorType) {
            return;
        }
        switch(monitorType) {
            case PatientMonitorType.PhilipsIntellivue:
            {
                onChange(_ => ({
                    deviceType: MedicalDeviceType.PatientMonitor,
                    monitorType: PatientMonitorType.PhilipsIntellivue,
                    serialPortName: undefined as any,
                    serialPortBaudRate: 19200
                } as Models.PhilipsIntellivueSettings));
                break;
            }
            case PatientMonitorType.SimulatedPhilipsIntellivue:
            {
                onChange(_ => ({
                    deviceType: MedicalDeviceType.PatientMonitor,
                    monitorType: PatientMonitorType.SimulatedPhilipsIntellivue
                } as Models.SimulatedPhilipsIntellivueSettings))
                break;
            }
            case PatientMonitorType.GEDash:
            {
                onChange(_ => ({
                    deviceType: MedicalDeviceType.PatientMonitor,
                    monitorType: PatientMonitorType.GEDash,
                    serialPortName: undefined as any,
                    serialPortBaudRate: 9600
                } as Models.GEDashSettings));
                break;
            }
        }
    }, [ onChange, value ])

    let monitorTypeSpecificFormControls: ReactNode = null;
    switch(value.monitorType) {
        case PatientMonitorType.PhilipsIntellivue:
            monitorTypeSpecificFormControls = (<PhilipsIntellivueSettingsFormControls
                value={value as Models.PhilipsIntellivueSettings}
                onChange={onChange}
            />);
            break;
        case PatientMonitorType.SimulatedPhilipsIntellivue:
            monitorTypeSpecificFormControls = (<SimulatedPhilipsIntellivueSettingsFormControls
                value={value as Models.SimulatedPhilipsIntellivueSettings}
                onChange={onChange}
            />);
            break;
        case PatientMonitorType.GEDash:
            monitorTypeSpecificFormControls = (<GEDashSettingsFormControls
                value={value as Models.GEDashSettings}
                onChange={onChange}
            />);
            break;
    }

    return (<>
        <FormGroup>
            <FormLabel>Monitor</FormLabel>
            <FormControl required
                as="select"
                value={value.monitorType}
                onChange={e => setMonitorType(e.target.value as PatientMonitorType)}
            >
                {Object.keys(PatientMonitorType).map(x => 
                    <option key={x} value={x}>{monitorNames[x] ?? x}</option>
                )}
            </FormControl>
        </FormGroup>
        {monitorTypeSpecificFormControls}
    </>)

}