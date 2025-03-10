import { useCallback, useMemo, useState } from "react";
import { Row, Col, Button, Spinner, Badge } from "react-bootstrap";
import { Models } from "../types/models";
import { AccordionCard } from "./AccordionCard";
import { deleteObject, sendPostRequest } from "../communication/ApiRequests";
import { PatientMonitorType } from "../types/enums";
import { DeleteButton } from "./DeleteButon";
import { CopyToUsbModal } from "./CopyToUsbModal";
import { confirmAlert } from "react-confirm-alert";
import { MonitorSettingsDescription } from "./MonitorSettingsDescription";
import { MonitorDataSettingsDescription } from "./MonitorDataSettingsDescription";
import { formatDate } from "../helpers/Formatters";

interface LogSessionListItemProps {
    logSession: Models.LogSession;
    numericsData: { [measurementType: string]: Models.DataExport.NumericsValue };
    onChange: (update: Update<Models.LogSession>) => void;
    onDeleted: () => void;
}

export const LogSessionListItem = (props: LogSessionListItemProps) => {

    const { logSession, numericsData, onChange, onDeleted } = props;
    const status = logSession.status;
    const monitorSettings = logSession.settings.monitorSettings;
    const monitorDataSettings = logSession.settings.monitorDataSettings;

    const [ isStartingStopping, setIsStartingStopping ] = useState<boolean>(false);
    const [ isDeleting, setIsDeleting ] = useState<boolean>(false);
    const [ showCopyToUsbModal, setShowCopyToUsbModal ] = useState<boolean>(false);

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

    const deleteLogSession = useCallback(async () => {
        setIsDeleting(true);
        await deleteObject(
            `api/log/${logSession.id}`, {},
            "Successfully deleted log session",
            "Could not delete log session",
            onDeleted,
            undefined,
            () => setIsDeleting(false)
        );
    }, [ logSession.id, onDeleted ]);

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
            {logSession.patientInfo
            ? <Col>
                <span className="text-nowrap">{logSession.patientInfo.firstName} {logSession.patientInfo.lastName}</span>
            </Col> : null}
            <Col>
                <span className="text-nowrap">{logSession.settings.name} ID: {logSession.id.substring(0, 6)}</span>
            </Col>
        </Row>}
    >
        <Row className="mb-2">
            <Col xs="auto">
                <MonitorSettingsDescription monitorSettings={monitorSettings} />
            </Col>
            <Col xs="auto">
                <MonitorDataSettingsDescription monitorDataSettings={monitorDataSettings} />
            </Col>
            <Col />
            <Col xs="auto">
                <DeleteButton
                    requireConfirm
                    confirmDialogTitle="Delete log session?"
                    confirmDialogMessage="Are you sure you want to delete this log session? Recorded data is preserved."
                    isDeleting={isDeleting}
                    onClick={deleteLogSession}
                    size="sm"
                    className="m-0"
                />
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
        {numericsData
        ? <Row>
            {Object.keys(numericsData).slice(0, 5).map(measurementType => {
                const numericsValue = numericsData[measurementType];
                return (<Col key={measurementType}>
                    <div>{measurementType}</div>
                    <div className="text-center align-items-end">
                        <span className="display-5">{numericsValue.value.toFixed(1)}</span>
                        <span className="text-secondary">{numericsValue.unit}</span>
                    </div>
                    <div>
                        <small>{formatDate(numericsValue.timestamp as unknown as string)}</small>
                    </div>
                </Col>);
            })}
        </Row> : null}
        <Row className="my-2">
            <Col></Col>
            <Col xs="auto">
                <Button
                    onClick={() => setShowCopyToUsbModal(true)}
                    size="lg"
                >
                    Copy to USB-drive
                </Button>
            </Col>
        </Row>
        <CopyToUsbModal
            show={showCopyToUsbModal}
            onClose={() => setShowCopyToUsbModal(false)}
            logSessionId={logSession.id}
        />
    </AccordionCard>);

}