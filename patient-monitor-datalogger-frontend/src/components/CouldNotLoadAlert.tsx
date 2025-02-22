import React from 'react';
import { Alert } from 'react-bootstrap';

interface CouldNotLoadAlertProps {
    errorText?: string;
}

export const CouldNotLoadAlert = (props: CouldNotLoadAlertProps) => {

    return (
        <Alert
            variant='danger'
        >
            {props.errorText ?? "Could not load"}
        </Alert>
    );

}