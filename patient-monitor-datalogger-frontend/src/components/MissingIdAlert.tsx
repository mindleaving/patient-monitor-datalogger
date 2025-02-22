import { Alert } from "react-bootstrap";

interface MissingIdAlertProps {}

export const MissingIdAlert = (props: MissingIdAlertProps) => {

    return (<Alert
        variant="danger"
    >
        Missing ID
    </Alert>);

}