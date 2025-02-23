import { useCallback, useMemo, useState } from "react";
import { Row, Col, Button, Spinner, Badge } from "react-bootstrap";
import { Models } from "../types/models";
import { AccordionCard } from "./AccordionCard";
import { deleteObject, sendPostRequest } from "../communication/ApiRequests";
import { PatientMonitorType } from "../types/enums";
import { DeleteButton } from "./DeleteButon";

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

    const [ isStartingStopping, setIsStartingStopping ] = useState<boolean>(false);
    const [ isDeleting, setIsDeleting ] = useState<boolean>(false);

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
    const stopLogging = async () => {
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
            <Col>
                ID: {logSession.id}
            </Col>
        </Row>}
    >
        {monitorSettings.type === PatientMonitorType.PhilipsIntellivue
        ? <Row className="mb-2">
            <Col xs="auto">
                Serial port: {(monitorSettings as Models.PhilipsIntellivuePatientMonitorSettings).serialPortName} @ {(monitorSettings as Models.PhilipsIntellivuePatientMonitorSettings).serialPortBaudRate} bit/s
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
        </Row> : null}
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
                    onClick={stopLogging}
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
                        <span className="display-5">{numericsValue.value}</span>
                        <span className="text-secondary">{numericsValue.unit}</span>
                    </div>
                    <div>
                        <small>{numericsValue.timestamp as any}</small>
                    </div>
                </Col>);
            })}
        </Row> : null}
    </AccordionCard>);

}