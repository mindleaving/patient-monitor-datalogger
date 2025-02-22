import { Alert } from 'react-bootstrap';

interface LoadingAlertProps {
    variant?: string;
}

export const LoadingAlert = (props: LoadingAlertProps) => {

    return (
        <Alert
            variant={props.variant ?? 'info'}
        >
            Loading...
        </Alert>
    );

}