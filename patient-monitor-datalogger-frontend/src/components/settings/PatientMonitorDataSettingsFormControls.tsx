import { useState, useMemo, useCallback } from "react";
import { FormGroup, FormLabel, FormCheck, Row, Col, Button, Alert } from "react-bootstrap";
import { waveTypeNames } from "../../helpers/Formatters";
import { MedicalDeviceType, PatientMonitorType, WaveType } from "../../types/enums";
import { Models } from "../../types/models";

interface PatientMonitorDataSettingsFormControlsProps {
    deviceSettings: Models.PatientMonitorSettings;
    value: Models.PatientMonitorDataSettings;
    onChange: (update: Update<Models.IMedicalDeviceDataSettings>) => void;
}
const getSerialBaudRate = (deviceSettings: Models.IMedicalDeviceSettings) => {
    if(deviceSettings.deviceType !== MedicalDeviceType.PatientMonitor) {
        return undefined;
    }
    const patientMonitorSettings = deviceSettings as Models.PatientMonitorSettings;
    switch(patientMonitorSettings.monitorType) {
        case PatientMonitorType.PhilipsIntellivue:
        {
            const philipsIntellivueSettings = patientMonitorSettings as Models.PhilipsIntellivueSettings;
            return philipsIntellivueSettings.serialPortBaudRate;
        }
        case PatientMonitorType.GEDash:
        {
            const geDashSettings = patientMonitorSettings as Models.GEDashSettings;
            return geDashSettings.serialPortBaudRate;
        }
        default:
            return undefined;
    }
}
const availableWaveTypes: WaveType[] = Object.values(WaveType).filter(x => x !== WaveType.Unknown);
export const PatientMonitorDataSettingsFormControls = (props: PatientMonitorDataSettingsFormControlsProps) => {

    const { deviceSettings, value, onChange } = props;

    const {
        includePatientInfo,
        includeAlerts,
        includeNumerics,
        includeWaves,
        selectedNumericsTypes,
        selectedWaveTypes
    } = value;

    const maxWaveCount = useMemo(() => {
        const serialPortBaudRate = getSerialBaudRate(deviceSettings);
        if(deviceSettings.monitorType === PatientMonitorType.PhilipsIntellivue 
            && !!serialPortBaudRate 
            && serialPortBaudRate <= 19200) { // Bandwidth supports no more than 3-4 waves
            return 4;
        }
        return undefined;
    }, [ deviceSettings ]);

    const updateProperty = useCallback((update: Update<Models.PatientMonitorDataSettings>) => {
        onChange(state => update(state as Models.PatientMonitorDataSettings))
    }, [ onChange ]);

    const toggleWaveType = (waveType: WaveType, isSelected: boolean) => {
        updateProperty(state => {
            if(isSelected) {
                if(state.selectedWaveTypes.includes(waveType)) {
                    return state;
                }
                return {
                    ...state,
                    selectedWaveTypes: state.selectedWaveTypes.concat(waveType)
                };
            } else {
                if(!state.selectedWaveTypes.includes(waveType)) {
                    return state;
                }
                return {
                    ...state,
                    selectedWaveTypes: state.selectedWaveTypes.filter(x => x !== waveType)
                };
            }
        });
    }

    const changeWavePriority = (waveType: WaveType, priority: number) => {
        if(priority < 0 || priority >= selectedWaveTypes.length) {
            return;
        }
        updateProperty(state => {
            const currentWavePriority = state.selectedWaveTypes.indexOf(waveType);
            if(currentWavePriority < 0) {
                return state;
            }
            const selectedWaveTypesCopy = [ ...state.selectedWaveTypes ];
            selectedWaveTypesCopy.splice(currentWavePriority, 1);
            selectedWaveTypesCopy.splice(priority, 0, waveType);
            return {
                ...state,
                selectedWaveTypes: selectedWaveTypesCopy
            };
        });
    }

    return (<FormGroup>
        <FormLabel>Parameters to be recorded</FormLabel>
        <div className="ms-3">
            <FormCheck
                checked={includePatientInfo}
                onChange={e => updateProperty(state => ({
                    ...state,
                    includePatientInfo: e.target.checked
                }))}
                label="Patient info"
            />
            <FormCheck
                required={!includeNumerics && !includeWaves}
                checked={includeAlerts}
                onChange={e => updateProperty(state => ({
                    ...state,
                    includeAlerts: e.target.checked
                }))}
                label="Alerts"
            />
            <FormCheck
                required={!includeAlerts && !includeWaves}
                checked={includeNumerics}
                onChange={e => updateProperty(state => ({
                    ...state,
                    includeNumerics: e.target.checked
                }))}
                label="Numerics (Heart rate, SpO2, respiration rate,...)"
            />
            <FormCheck
                required={!includeAlerts && !includeNumerics}
                checked={includeWaves}
                onChange={e => updateProperty(state => ({
                    ...state,
                    includeWaves: e.target.checked
                }))}
                label="Waves (ECG, Pleth,...)"
            />
            {includeWaves
            ? <div className="ms-3">
                {selectedWaveTypes.map((waveType,priority) => (
                    <Row className="my-2 align-items-center">
                        <Col xs="auto">
                            <Button
                                variant="primary"
                                onClick={() => changeWavePriority(waveType, priority - 1)}
                                disabled={priority === 0}
                                className="mx-2"
                                size="sm"
                            >
                                <div style={{ rotate: '-90deg' }}>-&gt;</div>
                            </Button>
                            <Button
                                variant="primary"
                                onClick={() => changeWavePriority(waveType, priority + 1)}
                                disabled={priority >= selectedWaveTypes.length - 1}
                                className="mx-2"
                                size="sm"
                            >
                                <div style={{ rotate: '90deg' }}>-&gt;</div>
                            </Button>
                        </Col>
                        <Col>
                            <FormCheck
                                checked={true}
                                onChange={e => toggleWaveType(waveType, e.target.checked)}
                                label={waveTypeNames[waveType] ?? waveType}
                            />
                        </Col>
                    </Row>
                ))}
                {maxWaveCount && selectedWaveTypes.length >= maxWaveCount
                ? <Alert variant="danger" className="py-1">
                    Selected baud rate doesn't support any more waves
                </Alert> : null}
                {availableWaveTypes.filter(waveType => !selectedWaveTypes.includes(waveType)).map(waveType => (
                    <FormCheck
                        key={waveType}
                        checked={false}
                        onChange={e => toggleWaveType(waveType, e.target.checked)}
                        label={waveTypeNames[waveType] ?? waveType}
                        disabled={maxWaveCount && selectedWaveTypes.length >= maxWaveCount}
                    />
                ))}
            </div> : null}
        </div>
    </FormGroup>);
}