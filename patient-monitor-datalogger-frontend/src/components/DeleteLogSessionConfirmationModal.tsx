import { Alert, Button, FormCheck, Modal } from "react-bootstrap";
import { AsyncButton } from "./AsyncButton";
import { useCallback, useEffect, useState } from "react";
import { deleteObject } from "../communication/ApiRequests";

interface DeleteLogSessionConfirmationModalProps {
    show: boolean;
    onClose: () => void;
    onLogSessionDeleted: (logSessionId: string) => void;
    logSessionId: string;
}

export const DeleteLogSessionConfirmationModal = (props: DeleteLogSessionConfirmationModalProps) => {

    const { show, onClose, onLogSessionDeleted, logSessionId } = props;

    const [ permanently, setPermanently ] = useState<boolean>(false);
    const [ permanentlyConfirmed, setPermanentlyConfirmed ] = useState<boolean>(false);
    const [ isDeleting, setIsDeleting ] = useState<boolean>(false);

    useEffect(() => {
        setPermanently(false);
    }, [ show]);

    useEffect(() => {
        if(permanently) {
            setPermanentlyConfirmed(false); // Ensure that user always has to confirm, if permanently changes
        }
    }, [ permanently ]);

    const deleteLogSession = useCallback(async () => {
        setIsDeleting(true);
        await deleteObject(
            `api/log/${logSessionId}`, permanently ? { 'permanently': 'true' } : {},
            "Successfully deleted log session",
            "Could not delete log session",
            () => {
                onLogSessionDeleted(logSessionId)
            },
            undefined,
            () => setIsDeleting(false)
        );
    }, [ onLogSessionDeleted, logSessionId, permanently ]);

    return (<Modal
        show={show}
        onHide={onClose}
    >
        <Modal.Header>
            <Modal.Title>{permanently ? "Delete" : "Hide"} log session?</Modal.Title>
        </Modal.Header>
        <Modal.Body>
            <span>
                Are you sure you want to hide / delete log session? <br />
            </span>
            <span className="text-secondary">
                <small>If not deleted permanently, it will only be hidden until the device is restarted.</small>
            </span>
            <div className="mt-2">
                <FormCheck
                    checked={permanently}
                    onChange={e => setPermanently(e.target.checked)}
                    label="Delete permanently"
                />
            </div>
            {permanently
            ? <Alert
                variant="danger"
            >
                ALL MEASUREMENTS WILL BE DELETED! ARE YOU SURE??? 
                <FormCheck
                    checked={permanentlyConfirmed}
                    onChange={e => setPermanentlyConfirmed(e.target.checked)}
                    label="YES!"
                />
            </Alert> : null}
        </Modal.Body>
        <Modal.Footer>
            <Button
                variant="secondary"
                onClick={onClose}
            >
                Cancel
            </Button>
            <AsyncButton
                variant="danger"
                isExecuting={isDeleting}
                onClick={deleteLogSession}
                activeText={permanently ? "Delete" : "Hide"}
                executingText={permanently ? "Deleting..." : "Hiding..."}
                disabled={permanently && !permanentlyConfirmed}
            />
        </Modal.Footer>
    </Modal>);

}