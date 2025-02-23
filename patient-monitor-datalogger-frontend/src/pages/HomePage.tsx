import { useEffect, useState } from "react";
import { Accordion, Button, Col, Row } from "react-bootstrap";
import { Models } from "../types/models";
import { buildLoadObjectFunc } from "../communication/ApiRequests";
import { LoadingAlert } from "../components/LoadingAlert";
import { LogSessionListItem } from "../components/LogSessionListItem";
import { NoEntriesAlert } from "../components/NoEntriesAlert";
import { useNavigate } from "react-router";
import { NumericsSignalRConnectionIndicator } from "../components/NumericsSignalRConnectionIndicator";

interface HomePageProps {
    logSessions: Models.LogSession[];
    setLogSessions: (update: Update<Models.LogSession[]>) => void;
}

export const HomePage = (props: HomePageProps) => {
    
    const { logSessions, setLogSessions } = props;

    const [ isLoadingLogSessions, setIsLoadingLogSessions ] = useState<boolean>(true);
    const [ numericsData, setNumericsData ] = useState<{ [logSessionId: string]: { [measurementType: string]: Models.DataExport.NumericsValue } }>({});
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

    const updateNumericsData = (newData: Models.DataExport.NumericsData) => {
        setNumericsData(state => ({
            ...state,
            [newData.logSessionId]: Object.assign(state[newData.logSessionId] ?? {}, newData.values)
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

    return (<>
        <Row className="align-items-center">
            <Col>
                <h1>Patient Monitor Data Logger</h1>
            </Col>
            <Col xs="auto">
                <NumericsSignalRConnectionIndicator 
                    onNewNumericsDataAvailable={updateNumericsData}
                    onPatientInfoAvailable={updatePatientInfo}
                    onLogStatusChanged={updateLogStatus}
                />
            </Col>
            <Col xs="auto">
                <Button
                    onClick={() => navigate('/create/session')}
                    size="lg"
                >
                    + Create data log session
                </Button>
            </Col>
        </Row>
        <Row className="my-2">
            <Col>
                {isLoadingLogSessions ? <LoadingAlert />
                : logSessions.length === 0 ? <NoEntriesAlert />
                : <Accordion>
                    {logSessions.map(logSession => (
                        <LogSessionListItem
                            key={logSession.id}
                            logSession={logSession}
                            numericsData={numericsData[logSession.id]}
                            onChange={update => updateLogSession(logSession.id, update)}
                            onDeleted={() => setLogSessions(state => state.filter(x => x.id !== logSession.id))}
                        />
                    ))}
                </Accordion>}
            </Col>
        </Row>
    </>);

}
export default HomePage;