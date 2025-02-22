import { FormEvent, useCallback, useEffect, useState } from "react";
import { loadObject, sendPostRequest } from "../communication/ApiRequests";
import { Models } from "../types/models";
import { PatientMonitorType } from "../types/enums";
import { Form, FormControl, FormGroup, FormLabel } from "react-bootstrap";
import { Center } from "../components/Center";
import { AsyncButton } from "../components/AsyncButton";
import { useNavigate } from "react-router";
import { showSuccessAlert } from "../helpers/AlertHelpers";

interface CreateNewLogSessionPageProps {
    onLogSessionCreated: (logSession: Models.LogSession) => void;
}

const monitorNames: { [key:string]: string } = {
    [PatientMonitorType.PhilipsIntellivue]: "Philips Intellivue",
    [PatientMonitorType.GEDash]: "GE Dash",
}
export const CreateNewLogSessionPage = (props: CreateNewLogSessionPageProps) => {

    const { onLogSessionCreated } = props;

    const [ selectedMonitorType, setSelectedMonitorType ] = useState<PatientMonitorType>(PatientMonitorType.PhilipsIntellivue);
    const [ availableSerialPorts, setAvailableSerialPorts ] = useState<string[]>([]);
    const [ serialPortName, setSerialPortName ] = useState<string>();
    const [ serialPortBaudRate, setSerialPortBaudRate ] = useState<number>(19200);
    const [ csvSeparator, setCsvSeparator ] = useState<string>(';');
    const [ isSubmitting, setIsSubmitting ] = useState<boolean>(false);
    const navigate = useNavigate();

    const createNewLogSession = async (e?: FormEvent) => {
        e?.preventDefault();
        if(!serialPortName) {
            return;
        }
        let monitorSettings: Models.IPatientMonitorSettings;
        switch(selectedMonitorType) {
            case PatientMonitorType.PhilipsIntellivue:
                monitorSettings = {
                    type: PatientMonitorType.PhilipsIntellivue,
                    serialPortName: serialPortName,
                    serialPortBaudRate: serialPortBaudRate
                } as Models.PhilipsIntellivuePatientMonitorSettings;
                break;
            case PatientMonitorType.GEDash:
                monitorSettings = {
                    type: PatientMonitorType.GEDash,
                    serialPortName: serialPortName,
                    serialPortBaudRate: serialPortBaudRate
                } as Models.GEDashPatientMonitorSettings;
                break;
            default:
                throw new Error(`Unsupported monitor type ${selectedMonitorType}`);
        }
        const logSessionSettings: Models.LogSessionSettings = {
            monitorSettings: monitorSettings,
            selectedNumericsTypes: [],
            selectedWaveTypes: [],
            csvSeparator: csvSeparator,
        };
        setIsSubmitting(true);
        await sendPostRequest(
            'api/log/start', {},
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
        <FormGroup>
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
        </FormGroup>
        <FormGroup>
            <FormLabel>Serial port - Baud rate</FormLabel>
            <FormControl
                as="select"
                value={serialPortBaudRate}
                onChange={e => setSerialPortBaudRate(Number(e.target.value))}
            >
                <option value="19200">19200 bits/s</option>
                <option value="115200">115200 bits/s</option>
            </FormControl>
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
        <Center className="mt-3">
            <AsyncButton
                type="submit"
                isExecuting={isSubmitting}
                activeText="Create"
                executingText="Create"
                size="lg"
            />
        </Center>
    </Form>
    </>);

}
export default CreateNewLogSessionPage;