import { useState, useCallback, useEffect } from "react";
import { FormGroup, FormLabel, FormControl } from "react-bootstrap";
import { loadObject } from "../../communication/ApiRequests";
import { Models } from "../../types/models";

interface GEDashSettingsFormControlsProps {
    value: Models.GEDashSettings;
    onChange: (update: Update<Models.IMedicalDeviceSettings>) => void;
}

export const GEDashSettingsFormControls = (props: GEDashSettingsFormControlsProps) => {

    const { value, onChange } = props;
    
    const [ availableSerialPorts, setAvailableSerialPorts ] = useState<string[]>([]);

    const loadAvailableSerialPorts = useCallback(async () => {
        await loadObject(
            'api/system/serialports', {},
            "Could not load available serial ports",
            setAvailableSerialPorts
        );
    }, []);

    const updateProperty = useCallback((update: Update<Models.GEDashSettings>) => {
        onChange(state => update(state as Models.GEDashSettings));
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
                value={value.serialPortName}
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
                value={value.serialPortBaudRate}
                onChange={e => updateProperty(state => ({
                    ...state,
                    serialPortBaudRate: Number(e.target.value)
                }))}
            >
                <option value="9600">9600 bits/s</option>
                <option value="19200">19200 bits/s</option>
                <option value="115200">115200 bits/s</option>
            </FormControl>
        </FormGroup>
    </>);

}