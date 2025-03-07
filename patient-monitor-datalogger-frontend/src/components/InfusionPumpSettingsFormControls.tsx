import { FormControl, FormGroup, FormLabel } from "react-bootstrap";
import { Models } from "../types/models";
import { InfusionPumpType, MedicalDeviceType } from "../types/enums";
import { infusionPumpSystemNames } from "../helpers/Formatters";
import { ReactNode, useCallback } from "react";
import { BBraunInfusionPumpSettingsFormControls } from "./BBraunInfusionPumpSettingsFormControls";

interface InfusionPumpSettingsFormControlsProps {
    value: Models.InfusionPumpSettings;
    onChange: (update: Update<Models.IMedicalDeviceSettings>) => void;
}

export const InfusionPumpSettingsFormControls = (props: InfusionPumpSettingsFormControlsProps) => {

    const { value, onChange } = props;

    let infusionPumpSystemSpecificFormControls: ReactNode = null;
    switch(value.infusionPumpType) {
        case InfusionPumpType.BBraunSpace:
        {
            infusionPumpSystemSpecificFormControls = (<BBraunInfusionPumpSettingsFormControls
                value={value as Models.BBraunInfusionPumpSettings}
                onChange={onChange}
            />);
            break;
        }
    }

    const setInfusionPumpType = useCallback((pumpType: InfusionPumpType) => {
        if(pumpType === value.infusionPumpType) {
            return;
        }
        switch(pumpType) {
            case InfusionPumpType.BBraunSpace:
            {
                onChange(_ => ({
                    deviceType: MedicalDeviceType.InfusionPumps,
                    infusionPumpType: InfusionPumpType.BBraunSpace,
                    hostname: '192.168.100.41',
                    port: 4001,
                    useCharacterStuffing: false,
                    pollPeriod: '00:00:10'
                } as Models.BBraunInfusionPumpSettings));
                break;
            }
        }
    }, [ onChange, value ]);

    return (<>
        <FormGroup>
            <FormLabel>System</FormLabel>
            <FormControl
                as="select"
                value={value.infusionPumpType}
                onChange={e => setInfusionPumpType(e.target.value as InfusionPumpType)}
            >
                {Object.keys(InfusionPumpType).map(x => (
                    <option key={x} value={x}>{infusionPumpSystemNames[x] ?? x}</option>
                ))}
            </FormControl>
        </FormGroup>
        {infusionPumpSystemSpecificFormControls}
    </>);

}