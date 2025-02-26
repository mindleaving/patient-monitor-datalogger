import { FormEvent, useCallback, useEffect, useState } from "react";
import { loadObject, sendPostRequest } from "../communication/ApiRequests";
import { Models } from "../types/models";
import { PatientMonitorType } from "../types/enums";
import { Form, FormCheck, FormControl, FormGroup, FormLabel } from "react-bootstrap";
import { Center } from "../components/Center";
import { AsyncButton } from "../components/AsyncButton";
import { useNavigate } from "react-router";
import { showSuccessAlert } from "../helpers/AlertHelpers";

interface CreateNewLogSessionPageProps {
    onLogSessionCreated: (logSession: Models.LogSession) => void;
}

const monitorNames: { [key:string]: string } = {
    [PatientMonitorType.PhilipsIntellivue]: "Philips Intellivue",
    [PatientMonitorType.SimulatedPhilipsIntellivue]: "Simulated Philips Intellivue",
    [PatientMonitorType.GEDash]: "GE Dash",
}
export const CreateNewLogSessionPage = (props: CreateNewLogSessionPageProps) => {

    const { onLogSessionCreated } = props;

    const [ selectedMonitorType, setSelectedMonitorType ] = useState<PatientMonitorType>(PatientMonitorType.PhilipsIntellivue);
    const [ availableSerialPorts, setAvailableSerialPorts ] = useState<string[]>([]);
    const [ serialPortName, setSerialPortName ] = useState<string>();
    const [ serialPortBaudRate, setSerialPortBaudRate ] = useState<number>(19200);
    const [ includeAlerts, setIncludeAlerts ] = useState<boolean>(false);
    const [ includeNumerics, setIncludeNumerics ] = useState<boolean>(true);
    const [ includeWaves, setIncludeWaves ] = useState<boolean>(false);
    const [ includePatientInfo, setIncludePatientInfo ] = useState<boolean>(true);
    const [ csvSeparator, setCsvSeparator ] = useState<string>(';');
    const [ isSubmitting, setIsSubmitting ] = useState<boolean>(false);
    const navigate = useNavigate();

    const createNewLogSession = async (e?: FormEvent) => {
        e?.preventDefault();
        let monitorSettings: Models.PatientMonitorSettings;
        switch(selectedMonitorType) {
            case PatientMonitorType.PhilipsIntellivue:
                if(!serialPortName) {
                    return;
                }
                monitorSettings = {
                    type: PatientMonitorType.PhilipsIntellivue,
                    serialPortName: serialPortName,
                    serialPortBaudRate: serialPortBaudRate
                } as Models.PhilipsIntellivuePatientMonitorSettings;
                break;
            case PatientMonitorType.SimulatedPhilipsIntellivue:
                monitorSettings = {
                    type: PatientMonitorType.SimulatedPhilipsIntellivue
                } as Models.SimulatedPhilipsIntellivuePatientMonitorSettings;
                break;
            case PatientMonitorType.GEDash:
                if(!serialPortName) {
                    return;
                }
                monitorSettings = {
                    type: PatientMonitorType.GEDash,
                    serialPortName: serialPortName,
                    serialPortBaudRate: serialPortBaudRate
                } as Models.GEDashPatientMonitorSettings;
                break;
            default:
                throw new Error(`Unsupported monitor type ${selectedMonitorType}`);
        }
        const monitorDataSettings: Models.MonitorDataSettings = {
            includeAlerts: includeAlerts,
            includeNumerics: includeNumerics,
            includeWaves: includeWaves,
            includePatientInfo: includePatientInfo,
            selectedNumericsTypes: [],
            selectedWaveTypes: [],
        };
        const logSessionSettings: Models.LogSessionSettings = {
            monitorSettings: monitorSettings,
            monitorDataSettings: monitorDataSettings,
            csvSeparator: csvSeparator,
        };
        setIsSubmitting(true);
        await sendPostRequest(
            'api/log', {},
            "Could not start log session",
            logSessionSettings,
            async response => {
                const logSession = await response.json() as Models.LogSession;
                onLogSessionCreated(logSession);
                showSuccessAlert("Successfully created log session");
                navigate('/');
            },
            undefined,
            () => setIsSubmitting(false)
        )
    }

    const loadAvailableSerialPorts = useCallback(async () => {
        await loadObject(
            'api/system/serialports', {},
            "Could not load available serial ports",
            setAvailableSerialPorts
        );
    }, []);

    useEffect(() => {
        loadAvailableSerialPorts();
    }, [ loadAvailableSerialPorts ]);

    useEffect(() => {
        if(availableSerialPorts.length > 0) {
            setSerialPortName(availableSerialPorts[0]);
        } else {
            setSerialPortName(undefined);
        }
    }, [ availableSerialPorts ]);

    return (<>
    <h1>Create new data log session</h1>
    <Form onSubmit={createNewLogSession}>
        <FormGroup>
            <FormLabel>Monitor</FormLabel>
            <FormControl required
                as="select"
                value={selectedMonitorType ?? ''}
                onChange={e => setSelectedMonitorType(e.target.value as PatientMonitorType)}
            >
                {Object.keys(PatientMonitorType).filter(x => x !== PatientMonitorType.Unknown).map(x => 
                    <option key={x} value={x}>{monitorNames[x]}</option>
                )}
            </FormControl>
        </FormGroup>
        {[ PatientMonitorType.PhilipsIntellivue, PatientMonitorType.GEDash ].includes(selectedMonitorType)
        ? <FormGroup>
            <FormLabel>Serial port - Name</FormLabel>
            <FormControl required
                as="select"
                value={serialPortName}
                onChange={e => setSerialPortName(e.target.value)}
            >
                {availableSerialPorts.map(name => (
                    <option key={name} value={name}>{name}</option>
                ))}
            </FormControl>
        </FormGroup> : null}
        {[ PatientMonitorType.PhilipsIntellivue, PatientMonitorType.GEDash ].includes(selectedMonitorType)
        ? <FormGroup>
            <FormLabel>Serial port - Baud rate</FormLabel>
            <FormControl
                as="select"
                value={serialPortBaudRate}
                onChange={e => setSerialPortBaudRate(Number(e.target.value))}
            >
                <option value="19200">19200 bits/s</option>
                <option value="115200">115200 bits/s</option>
            </FormControl>
        </FormGroup> : null}
        <FormGroup>
            <FormLabel>Parameters to be recorded</FormLabel>
            <div className="ms-3">
                <FormCheck
                    required={!includeNumerics && !includeWaves}
                    checked={includeAlerts}
                    onChange={e => setIncludeAlerts(e.target.checked)}
                    label="Alerts"
                />
                <FormCheck
                    required={!includeAlerts && !includeWaves}
                    checked={includeNumerics}
                    onChange={e => setIncludeNumerics(e.target.checked)}
                    label="Numerics (Heart rate, SpO2, respiration rate,...)"
                />
                <FormCheck
                    required={!includeAlerts && !includeNumerics}
                    checked={includeWaves}
                    onChange={e => setIncludeWaves(e.target.checked)}
                    label="Waves (ECG, Pleth,...)"
                />
                <FormCheck
                    checked={includePatientInfo}
                    onChange={e => setIncludePatientInfo(e.target.checked)}
                    label="Patient info"
                />
            </div>
        </FormGroup>
        <FormGroup>
            <FormLabel>CSV separator</FormLabel>
            <FormControl
                as="select"
                value={csvSeparator}
                onChange={e => setCsvSeparator(e.target.value)}
            >
                <option value=",">Comma (,)</option>
                <option value=";">Semi-colon (;)</option>
            </FormControl>
        </FormGroup>
        <Center className="my-3">
            <AsyncButton
                type="submit"
                isExecuting={isSubmitting}
                activeText="Create"
                executingText="Create"
                size="lg"
                disabled={
                    ([ PatientMonitorType.PhilipsIntellivue, PatientMonitorType.GEDash ].includes(selectedMonitorType) && !serialPortName)
                    || (!includeAlerts && !includeNumerics && !includeWaves)}
            />
        </Center>
    </Form>
    </>);

}
export default CreateNewLogSessionPage;