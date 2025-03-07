import { FormCheck, FormControl, FormGroup, FormLabel, InputGroup } from "react-bootstrap";
import { Models } from "../types/models";
import { useCallback, useEffect, useState } from "react";

interface BBraunInfusionPumpSettingsFormControlsProps {
    value: Models.BBraunInfusionPumpSettings;
    onChange: (update: Update<Models.IMedicalDeviceSettings>) => void;
}

export const BBraunInfusionPumpSettingsFormControls = (props: BBraunInfusionPumpSettingsFormControlsProps) => {

    const { value, onChange } = props;

    const [ pollPeriodInSeconds, setPollPeriodInSeconds ] = useState<number>(10);

    const updateProperty = useCallback((update: Update<Models.BBraunInfusionPumpSettings>) => {
        onChange(state => update(state as Models.BBraunInfusionPumpSettings));
    }, [ onChange]);

    useEffect(() => {
        const minutes = Math.floor(pollPeriodInSeconds / 60);
        const seconds = pollPeriodInSeconds - minutes * 60;
        const timeSpan = `00:${(minutes + '').padStart(2, '0')}:${(seconds + '').padStart(2, '0')}`
        updateProperty(state => ({
            ...state,
            pollPeriod: timeSpan
        }));
    }, [ pollPeriodInSeconds, updateProperty ]);

    return (<>
        <FormGroup>
            <FormLabel>Hostname / IP address</FormLabel>
            <FormControl
                value={value.hostname ?? ''}
                onChange={e => updateProperty(state => ({
                    ...state,
                    hostname: e.target.value
                }))}
            />
        </FormGroup>
        <FormGroup>
            <FormLabel>Port</FormLabel>
            <FormControl
                type="number"
                value={value.port!}
                onChange={e => updateProperty(state => ({
                    ...state,
                    port: Number(e.target.value)
                }))}
            />
        </FormGroup>
        <FormGroup className="pt-3 pb-1">
            <FormCheck
                checked={value.useCharacterStuffing}
                onChange={e => updateProperty(state => ({
                    ...state,
                    useCharacterStuffing: e.target.checked
                }))}
                label="Use Character Stuffing (must match BCC protocol settings of SpaceStation)"
            />
        </FormGroup>
        <FormGroup>
        <FormLabel>Poll Period</FormLabel>
            <InputGroup>
                <FormControl
                    type="number"
                    defaultValue={pollPeriodInSeconds ?? 10}
                    min={5}
                    onBlur={e => setPollPeriodInSeconds(Number(e.target.value))}
                />
                <InputGroup.Text>seconds</InputGroup.Text>
            </InputGroup>
        </FormGroup>
    </>);

}