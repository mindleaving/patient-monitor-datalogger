import { useState } from "react";
import { Row, Col, Button, Spinner, Badge } from "react-bootstrap";
import { Models } from "../types/models";
import { AccordionCard } from "./AccordionCard";
import { sendPostRequest } from "../communication/ApiRequests";
import { CopyToUsbModal } from "./CopyToUsbModal";
import { confirmAlert } from "react-confirm-alert";
import { DeviceSettingsDescription } from "./DeviceSettingsDescription";
import { DeviceDataSettingsDescription } from "./DeviceDataSettingsDescription";
import { formatDate } from "../helpers/Formatters";
import { DeviceDescription } from "./DeviceDescription";
import { DeleteLogSessionConfirmationModal } from "./DeleteLogSessionConfirmationModal";
import { CustomEventModal } from "./CustomEventModal";

interface LogSessionListItemProps {
    logSession: Models.LogSession;
    observations: Models.DataExport.Observation[];
    onChange: (update: Update<Models.LogSession>) => void;
    onDeleted: () => void;
}

export const LogSessionListItem = (props: LogSessionListItemProps) => {

    const { logSession, observations, onChange, onDeleted } = props;
    const status = logSession.status;
    const deviceSettings = logSession.settings.deviceSettings;
    const deviceDataSettings = logSession.settings.dataSettings;

    const [ isStartingStopping, setIsStartingStopping ] = useState<boolean>(false);
    const [ showDeleteModal, setShowDeleteModal ] = useState<boolean>(false);
    const [ showCopyToUsbModal, setShowCopyToUsbModal ] = useState<boolean>(false);
    const [ showCustomEventModal, setShowCustomEventModal ] = useState<boolean>(false);

    const startLogging = async () => {
        setIsStartingStopping(true);
        await sendPostRequest(
            `api/log/${logSession.id}/start`, {},
            "Could not start recording",
            null,
            async response => {
                const newStatus = await response.json() as Models.LogStatus;
                onChange(state => ({
                    ...state,
                    status: newStatus
                }));
            },
            undefined,
            () => setIsStartingStopping(false)
        );
    }
    const stopLogging = async (force: boolean = false) => {
        if(!force) {
            confirmAlert({
                title: "Stop recording?",
                message: "Are you sure you want to stop recording?",
                closeOnClickOutside: true,
                buttons: [
                    {
                        label: "No, cancel"
                    },
                    {
                        label: "Yes, stop recording",
                        onClick: () => stopLogging(true)
                    }
                ]
            });
            return;
        }
        setIsStartingStopping(true);
        await sendPostRequest(
            `api/log/${logSession.id}/stop`, {},
            "Could not stop recording",
            null,
            async response => {
                const newStatus = await response.json() as Models.LogStatus;
                onChange(state => ({
                    ...state,
                    status: newStatus
                }));
            },
            undefined,
            () => setIsStartingStopping(false)
        );
    }

    return (<AccordionCard
        eventKey={logSession.id}
        title={<Row className="align-items-center">
            <Col xs="auto">
                <Badge pill 
                    bg={logSession.status.isRunning ? 'success' : 'danger'}
                    text="light"
                >
                    {logSession.status.isRunning ? 'Recording' : 'Stopped'}
                </Badge>
            </Col>
            <Col xs="auto">
                <span className="text-nowrap">{logSession.settings.name} ID: {logSession.id.substring(0, 6)}</span>
            </Col>
            <Col xs="auto">
                <DeviceDescription deviceSettings={logSession.settings.deviceSettings} />
            </Col>
            {logSession.patientInfo
            ? <Col xs="auto">
                <span className="text-nowrap">{logSession.patientInfo.firstName} {logSession.patientInfo.lastName}</span>
            </Col> : null}
            <Col />
        </Row>}
    >
        <Row className="mb-2">
            <Col xs="auto">
                <DeviceSettingsDescription deviceSettings={deviceSettings} />
            </Col>
            <Col xs="auto">
                <DeviceDataSettingsDescription deviceDataSettings={deviceDataSettings} />
            </Col>
            <Col />
            <Col xs="auto">
                <Button
                    onClick={() => setShowDeleteModal(true)}
                    variant="danger"
                    size="sm"
                >
                    Delete
                </Button>
            </Col>
        </Row>
        <Row>
            <Col>
                <Button
                    variant="success"
                    onClick={startLogging}
                    className="w-100 mx-3"
                    style={{ height: '200px' }}
                    disabled={isStartingStopping || status.isRunning}
                >
                    <Row className="align-items-center">
                        {isStartingStopping 
                        ? <Col xs="auto">
                            <Spinner />
                        </Col> : null}
                        <Col>
                            <span className="display-1">Start</span>
                        </Col>
                    </Row>
                </Button>
            </Col>
            <Col>
                <Button
                    variant="danger"
                    onClick={() => stopLogging()}
                    className="w-100 mx-3"
                    style={{ height: '200px' }}
                    disabled={isStartingStopping || !status.isRunning}
                >
                    <Row className="align-items-center">
                        {isStartingStopping 
                        ? <Col xs="auto">
                            <Spinner />
                        </Col> : null}
                        <Col>
                            <span className="display-1">Stop</span>
                        </Col>
                    </Row>
                </Button>
            </Col>
        </Row>
        {observations
        ? <Row>
            {observations.slice(0, 5).map((observation,observationIndex) => {
                return (<Col key={observationIndex}>
                    <div>{observation.parameterName}</div>
                    <div className="text-center align-items-end">
                        <span className="display-5">{observation.value}</span>
                        <span className="text-secondary">{observation.unit}</span>
                    </div>
                    <div>
                        <small>{formatDate(observation.timestamp)}</small>
                    </div>
                </Col>);
            })}
        </Row> : null}
        <Row className="my-2">
            <Col></Col>
            <Col xs="auto">
                <Button
                    onClick={() => setShowCustomEventModal(true)}
                    size="lg"
                >
                    Create custom event
                </Button>
            </Col>
            <Col xs="auto">
                <Button
                    onClick={() => setShowCopyToUsbModal(true)}
                    size="lg"
                >
                    Copy to USB-drive
                </Button>
            </Col>
        </Row>
        <CustomEventModal
            show={showCustomEventModal}
            onClose={() => setShowCustomEventModal(false)}
            logSessionId={logSession.id}
        />
        <CopyToUsbModal
            show={showCopyToUsbModal}
            onClose={() => setShowCopyToUsbModal(false)}
            logSessionId={logSession.id}
        />
        <DeleteLogSessionConfirmationModal
            show={showDeleteModal}
            onClose={() => setShowDeleteModal(false)}
            logSessionId={logSession.id}
            onLogSessionDeleted={onDeleted}
        />
    </AccordionCard>);

}