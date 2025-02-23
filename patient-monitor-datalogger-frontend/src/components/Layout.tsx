import { Col, Container, Row } from "react-bootstrap";
import { Outlet } from "react-router";
import { ToastContainer } from "react-toastify";

type LayoutProps = {};

export const Layout = (props: LayoutProps) => {

    return (<>
    <ToastContainer theme="colored" />
    <Container>
        <Row className="mt-2">
            <Col>
                <Outlet />
            </Col>
        </Row>
    </Container>
    </>);

}