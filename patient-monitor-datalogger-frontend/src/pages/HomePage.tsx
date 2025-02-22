import { useEffect, useState } from "react";
import { Accordion, Button, Col, Row } from "react-bootstrap";
import { Models } from "../types/models";
import { buildLoadObjectFunc } from "../communication/ApiRequests";
import { LoadingAlert } from "../components/LoadingAlert";
import { LogSessionListItem } from "../components/LogSessionListItem";
import { NoEntriesAlert } from "../components/NoEntriesAlert";
import { useNavigate } from "react-router";

interface HomePageProps {
    logSessions: Models.LogSession[];
    setLogSessions: (update: Update<Models.LogSession[]>) => void;
}

export const HomePage = (props: HomePageProps) => {
    
    const { logSessions, setLogSessions } = props;

    const [ isLoadingLogSessions, setIsLoadingLogSessions ] = useState<boolean>(true);
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
    }, []);

    const updateLogSession = (logSessionId: string, update: Update<Models.LogSession>) => {
        setLogSessions(state => state.map(logSession => logSession.id === logSessionId ? update(logSession) : logSession));
    }
    

    return (<>
        <Row className="align-items-center">
            <Col>
                <h1>Patient Monitor Data Logger</h1>
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
                            onChange={update => updateLogSession(logSession.id, update)}
                        />
                    ))}
                </Accordion>}
            </Col>
        </Row>
    </>);

}
export default HomePage;