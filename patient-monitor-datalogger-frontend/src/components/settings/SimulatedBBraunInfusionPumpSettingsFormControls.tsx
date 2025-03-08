import { FormControl, FormGroup, FormLabel, InputGroup } from "react-bootstrap";
import { Models } from "../../types/models";
import { useCallback, useEffect, useState } from "react";

interface SimulatedBBraunInfusionPumpSettingsFormControlsProps {
    value: Models.SimulatedBBraunInfusionPumpSettings;
    onChange: (update: Update<Models.IMedicalDeviceSettings>) => void;
}

export const SimulatedBBraunInfusionPumpSettingsFormControls = (props: SimulatedBBraunInfusionPumpSettingsFormControlsProps) => {

    const { value, onChange } = props;

    const [ pollPeriodInSeconds, setPollPeriodInSeconds ] = useState<number>(10);
    
    const updateProperty = useCallback((update: Update<Models.SimulatedBBraunInfusionPumpSettings>) => {
        onChange(state => update(state as Models.SimulatedBBraunInfusionPumpSettings));
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
            <FormLabel>Bed ID</FormLabel>
            <FormControl
                value={value.bedId}
                onChange={e => updateProperty(state => ({
                    ...state,
                    bedId: e.target.value
                }))}
            />
        </FormGroup>
        <FormGroup>
            <FormLabel>Number of pillars</FormLabel>
            <FormControl
                type="number"
                min={1}
                max={3}
                value={value.pillarCount}
                onChange={e => updateProperty(state => ({
                    ...state,
                    pillarCount: Number(e.target.value)
                }))}
            />
        </FormGroup>
        <FormGroup>
            <FormLabel>Number of pumps</FormLabel>
            <FormControl
                type="number"
                min={0}
                max={24}
                value={value.pumpCount}
                onChange={e => updateProperty(state => ({
                    ...state,
                    pumpCount: Number(e.target.value)
                }))}
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