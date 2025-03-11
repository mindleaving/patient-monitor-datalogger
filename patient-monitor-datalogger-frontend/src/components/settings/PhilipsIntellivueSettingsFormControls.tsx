import { FormGroup, FormLabel, FormControl } from "react-bootstrap";
import { Models } from "../../types/models";
import { useCallback, useEffect, useState } from "react";
import { loadObject } from "../../communication/ApiRequests";

interface PhilipsIntellivueSettingsFormControlsProps {
    value: Models.PhilipsIntellivueSettings;
    onChange: (update: Update<Models.IMedicalDeviceSettings>) => void;
}

export const PhilipsIntellivueSettingsFormControls = (props: PhilipsIntellivueSettingsFormControlsProps) => {

    const { value, onChange } = props;
    
    const [ availableSerialPorts, setAvailableSerialPorts ] = useState<string[]>([]);

    const loadAvailableSerialPorts = useCallback(async () => {
        await loadObject(
            'api/system/serialports', {},
            "Could not load available serial ports",
            setAvailableSerialPorts
        );
    }, []);

    const updateProperty = useCallback((update: Update<Models.PhilipsIntellivueSettings>) => {
        onChange(state => update(state as Models.PhilipsIntellivueSettings));
    }, [ onChange ]);

    useEffect(() => {
        loadAvailableSerialPorts();
    }, [ loadAvailableSerialPorts ]);

    useEffect(() => {
        if(availableSerialPorts.length > 0) {
            updateProperty(state => ({
                ...state,
                serialPortName: availableSerialPorts[0]
            }));
        } else {
            updateProperty(state => ({
                ...state,
                serialPortName: undefined as any
            }))
        }
    }, [availableSerialPorts, updateProperty]);

    return (<>
        <FormGroup>
            <FormLabel>Serial port - Name</FormLabel>
            <FormControl required
                as="select"
                value={value.serialPortName ?? ''}
                onChange={e => updateProperty(state => ({
                    ...state,
                    serialPortName: e.target.value
                }))}
            >
                {availableSerialPorts.map(name => (
                    <option key={name} value={name}>{name}</option>
                ))}
            </FormControl>
        </FormGroup>
        <FormGroup>
            <FormLabel>Serial port - Baud rate</FormLabel>
            <FormControl
                as="select"
                value={value.serialPortBaudRate ?? 19200}
                onChange={e => updateProperty(state => ({
                    ...state,
                    serialPortBaudRate: Number(e.target.value)
                }))}
            >
                <option value="19200">19200 bits/s</option>
                <option value="115200">115200 bits/s</option>
            </FormControl>
        </FormGroup>
    </>);

}