import { useCallback, useEffect, useMemo, useState } from "react";
import { Accordion, Button, Col, Row } from "react-bootstrap";
import { Models } from "../types/models";
import { buildLoadObjectFunc } from "../communication/ApiRequests";
import { LoadingAlert } from "../components/LoadingAlert";
import { LogSessionListItem } from "../components/LogSessionListItem";
import { NoEntriesAlert } from "../components/NoEntriesAlert";
import { useNavigate } from "react-router";
import { NumericsSignalRConnectionIndicator } from "../components/NumericsSignalRConnectionIndicator";
import { MedicalDeviceType } from "../types/enums";
import { MenuModal } from "../components/MenuModal";
import { compareLogSessions } from "../helpers/sortHelpers";

interface HomePageProps {
    logSessions: Models.LogSession[];
    setLogSessions: (update: Update<Models.LogSession[]>) => void;
}
export const HomePage = (props: HomePageProps) => {
    
    const { logSessions, setLogSessions } = props;

    const sortedLogSessions = useMemo(() => [...logSessions].sort((a,b) => compareLogSessions(a, b)), [ logSessions]);
    const [ isLoadingLogSessions, setIsLoadingLogSessions ] = useState<boolean>(true);
    const [ observations, setObservations ] = useState<{ [logSessionId: string]: Models.DataExport.Observation[] }>({});
    const [ showMenu, setShowMenu ] = useState<boolean>(false);
    const navigate = useNavigate();

    useEffect(() => {
        setIsLoadingLogSessions(true);
        const loadLogSessions = buildLoadObjectFunc(
            'api/log/sessions', {},
            "Could not load log sessions",
            setLogSessions,
            undefined,
            () => setIsLoadingLogSessions(false)
        );
        loadLogSessions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const updateLogSession = (logSessionId: string, update: Update<Models.LogSession>) => {
        setLogSessions(state => state.map(logSession => logSession.id === logSessionId ? update(logSession) : logSession));
    }

    const updateObservations = (newData: Models.DataExport.LogSessionObservations) => {
        setObservations(state => ({
            ...state,
            [newData.logSessionId]: newData.observations.concat(state[newData.logSessionId] ?? []).slice(0, 5)
        }));
    }

    const updatePatientInfo = (patientInfo: Models.PatientInfo) => {
        updateLogSession(patientInfo.logSessionId, logSession => ({
            ...logSession,
            patientInfo: patientInfo
        }));
    }

    const updateLogStatus = (logStatus: Models.LogStatus) => {
        updateLogSession(logStatus.logSessionId, logSession => ({
            ...logSession,
            status: logStatus
        }));
    }

    const closeMenu = useCallback(() => {
        setShowMenu(false);
    }, []);

    return (<>
        <Row className="align-items-center">
            <Col>
                <h1>Medical Device Data Logger</h1>
            </Col>
            <Col xs="auto">
                <NumericsSignalRConnectionIndicator 
                    onNewObservationsAvailable={updateObservations}
                    onPatientInfoAvailable={updatePatientInfo}
                    onLogStatusChanged={updateLogStatus}
                />
            </Col>
            <Col xs="auto">
                <Button
                    onClick={() => setShowMenu(true)}
                >
                    Menu
                </Button>
            </Col>
        </Row>
        <Row className="my-2">
            <Col>
                {isLoadingLogSessions ? <LoadingAlert />
                : sortedLogSessions.length === 0 ? <NoEntriesAlert />
                : <Accordion>
                    {sortedLogSessions.map(logSession => (
                        <LogSessionListItem
                            key={logSession.id}
                            logSession={logSession}
                            observations={observations[logSession.id]}
                            onChange={update => updateLogSession(logSession.id, update)}
                            onDeleted={() => setLogSessions(state => state.filter(x => x.id !== logSession.id))}
                        />
                    ))}
                </Accordion>}
            </Col>
        </Row>
        <MenuModal
            show={showMenu}
            onClose={closeMenu}
            isAnyLogSessionRunning={logSessions.some(x => x.status.isRunning)}
        />
    </>);

}
export default HomePage;