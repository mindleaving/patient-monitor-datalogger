import { FormEvent, useState } from "react";
import { sendPostRequest } from "../communication/ApiRequests";
import { Models } from "../types/models";
import { MedicalDeviceType, PatientMonitorType, WaveType } from "../types/enums";
import { Form, FormControl, FormGroup, FormLabel } from "react-bootstrap";
import { Center } from "../components/Center";
import { AsyncButton } from "../components/AsyncButton";
import { useNavigate } from "react-router";
import { showSuccessAlert } from "../helpers/AlertHelpers";
import { DeviceSettingsFormControls } from "../components/settings/DeviceSettingsFormControls";
import { DeviceDataSettingsFormControls } from "../components/settings/DeviceDataSettingsFormControls";

interface CreateNewLogSessionPageProps {
    onLogSessionCreated: (logSession: Models.LogSession) => void;
}

export const CreateNewLogSessionPage = (props: CreateNewLogSessionPageProps) => {

    const { onLogSessionCreated } = props;

    const [ sessionName, setSessionName ] = useState<string>('');
    const [ deviceSettings, setDeviceSettings ] = useState<Models.IMedicalDeviceSettings>({ 
        deviceType: MedicalDeviceType.PatientMonitor,
        monitorType: PatientMonitorType.PhilipsIntellivue,
        serialPortName: undefined as any,
        serialPortBaudRate: 19200
    } as Models.PhilipsIntellivueSettings);
    const [ dataSettings, setDataSettings ] = useState<Models.IMedicalDeviceDataSettings>({ 
        deviceType: MedicalDeviceType.PatientMonitor,
        includePatientInfo: true,
        includeAlerts: false,
        includeNumerics: true,
        includeWaves: false,
        selectedNumericsTypes: [],
        selectedWaveTypes: [
            WaveType.ArterialBloodPressure,
            WaveType.Pleth,
            WaveType.Respiration,
            WaveType.EcgDefault
        ]
    } as Models.PatientMonitorDataSettings);
    const [ csvSeparator, setCsvSeparator ] = useState<string>(';');
    const [ isSubmitting, setIsSubmitting ] = useState<boolean>(false);
    const navigate = useNavigate();

    const createNewLogSession = async (e?: FormEvent) => {
        e?.preventDefault();
        const logSessionSettings: Models.LogSessionSettings = {
            name: sessionName,
            deviceSettings: deviceSettings,
            dataSettings: dataSettings,
            csvSeparator: csvSeparator
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

    return (<>
    <h1>Create new data log session</h1>
    <Form onSubmit={createNewLogSession}>
        <FormGroup>
            <FormLabel>Name</FormLabel>
            <FormControl
                value={sessionName}
                onChange={e => setSessionName(e.target.value)}
            />
        </FormGroup>
        <DeviceSettingsFormControls
            value={deviceSettings}
            onChange={setDeviceSettings}
        />
        <DeviceDataSettingsFormControls
            deviceSettings={deviceSettings}
            value={dataSettings}
            onChange={setDataSettings}
        />
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
            />
        </Center>
    </Form>
    </>);

}
export default CreateNewLogSessionPage;