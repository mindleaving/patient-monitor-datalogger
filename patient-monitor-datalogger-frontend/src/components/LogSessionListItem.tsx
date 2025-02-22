import { useState } from "react";
import { Row, Col, Button, Spinner } from "react-bootstrap";
import { Models } from "../types/models";
import { AccordionCard } from "./AccordionCard";

interface LogSessionListItemProps {
    logSession: Models.LogSession;
    onChange: (update: Update<Models.LogSession>) => void;
}

export const LogSessionListItem = (props: LogSessionListItemProps) => {

    const { logSession, onChange } = props;
    const status = logSession.status;

    const [ isStartingStopping, setIsStartingStopping ] = useState<boolean>(false);

    const startLogging = async () => {
        setIsStartingStopping(true);
        onChange(state => ({
            ...state,
            status: {
                ...state.status,
                isRunning: true
            }
        }));
        setTimeout(() => setIsStartingStopping(false), 1000);
    }
    const stopLogging = async () => {
        setIsStartingStopping(true);
        onChange(state => ({
            ...state,
            status: {
                ...state.status,
                isRunning: false
            }
        }));
        setTimeout(() => setIsStartingStopping(false), 1000);
    }

    return (<AccordionCard
        eventKey={logSession.id}
        title={logSession.id}
    >
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
    </AccordionCard>);

}