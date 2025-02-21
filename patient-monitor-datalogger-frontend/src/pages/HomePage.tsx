import { Button, Col, Row } from "react-bootstrap";

export const HomePage = () => {

    const startLogging = async () => {

    }

    return (<>
        <h1>Patient Monitor Data Logger</h1>
       <Row>
            <Col>
                <Button
                    variant="success"
                    onClick={startLogging}
                    size="lg"
                >
                    Start
                </Button>
            </Col>
            <Col>
            
            </Col>
        </Row>
        <Row>
            <Col>
                
            </Col>
        </Row>
    </>);

}
export default HomePage;