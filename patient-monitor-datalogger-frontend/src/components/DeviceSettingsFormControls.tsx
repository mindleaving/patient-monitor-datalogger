import { ReactNode, useCallback } from "react";
import { InfusionPumpType, MedicalDeviceType, PatientMonitorType } from "../types/enums";
import { Models } from "../types/models";
import { PatientMonitorSettingsFormControls } from "./PatientMonitorSettingsFormControls";
import { deviceTypeNames } from "../helpers/Formatters";
import { FormGroup, FormLabel, FormControl } from "react-bootstrap";
import { InfusionPumpSettingsFormControls } from "./InfusionPumpSettingsFormControls";

interface DeviceSettingsFormControlsProps {
    value: Models.IMedicalDeviceSettings;
    onChange: (update: Update<Models.IMedicalDeviceSettings>) => void;
}

export const DeviceSettingsFormControls = (props: DeviceSettingsFormControlsProps) => {

    const { value, onChange } = props;

    let deviceSpecificFormControls: ReactNode = null;
    switch(value.deviceType) {
        case MedicalDeviceType.PatientMonitor:
        {
            deviceSpecificFormControls = (<PatientMonitorSettingsFormControls
                value={value as Models.PatientMonitorSettings}
                onChange={onChange}
            />);
            break;
        }
        case MedicalDeviceType.InfusionPumps:
        {
            deviceSpecificFormControls = (<InfusionPumpSettingsFormControls
                value={value as Models.InfusionPumpSettings}
                onChange={onChange}
            />);
            break;
        }
    }

    const setDeviceType = useCallback((deviceType: MedicalDeviceType) => {
        if(deviceType === value.deviceType) {
            return;
        }
        switch(deviceType) {
            case MedicalDeviceType.PatientMonitor:
            {
                onChange(state => ({
                    ...state,
                    deviceType: deviceType,
                    monitorType: PatientMonitorType.PhilipsIntellivue,
                    serialPortName: undefined as any,
                    serialPortBaudRate: 19200
                } as Models.PhilipsIntellivueSettings));
                break;
            }
            case MedicalDeviceType.InfusionPumps:
            {
                onChange(state => ({
                    ...state,
                    deviceType: deviceType,
                    infusionPumpType: InfusionPumpType.BBraunSpace,
                    hostname: '192.168.100.41',
                    port: 4001,
                    pollPeriod: '00:00:10',
                    useCharacterStuffing: false
                } as Models.BBraunInfusionPumpSettings));
                break;
            }
        }
    }, [ onChange, value ]);

    return (<>
        <FormGroup>
            <FormLabel>Device group</FormLabel>
            <FormControl required
                as="select"
                value={value.deviceType ?? ''}
                onChange={e => setDeviceType(e.target.value as MedicalDeviceType)}
            >
                {Object.keys(MedicalDeviceType).map(x => 
                    <option key={x} value={x}>{deviceTypeNames[x] ?? x}</option>
                )}
            </FormControl>
        </FormGroup>
        {deviceSpecificFormControls}
    </>)
}

