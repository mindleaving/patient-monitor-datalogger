import React, { PropsWithChildren } from 'react';
import { Row, Col } from 'react-bootstrap';

interface CenterProps {
    className?: string,
}

export const Center = (props: PropsWithChildren<CenterProps>) => {

    return (
        <Row className={props.className}>
            <Col />
            <Col xs="auto">{props.children}</Col>
            <Col />
        </Row>
    );

}