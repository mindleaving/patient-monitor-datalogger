import { Button, FormControl, FormGroup, FormLabel, Modal } from "react-bootstrap";
import { AsyncButton } from "./AsyncButton";
import { useCallback, useEffect, useState } from "react";
import { Models } from "../types/models";
import { sendPostRequest } from "../communication/ApiRequests";
import { showSuccessAlert } from "../helpers/AlertHelpers";

interface CustomEventModalProps {
    show: boolean;
    onClose: () => void;
    logSessionId: string;
}

export const CustomEventModal = (props: CustomEventModalProps) => {

    const { show, onClose, logSessionId } = props;

    const [ preconfiguredMessages, setPreconfiguredMessages ] = useState<string[]>([]);
    const [ hasTriedToLoadPreconfiguredMessages, setHasTriedToLoadPreconfiguredMessages ] = useState<boolean>(false);
    const [ message, setMessage ] = useState<string>('');
    const [ isSubmitting, setIsSubmitting ] = useState<boolean>(false);

    useEffect(() => {
        if(!show) {
            return;
        }
        if(hasTriedToLoadPreconfiguredMessages) {
            return;
        }
        const loadPreconfiguredMessages = async () => {
            try {
                const response = await fetch('custom-event-preconfigured-messages.json');
                if(response.ok) {
                    const content = await response.json() as string[];
                    setPreconfiguredMessages(content);
                }
            } catch {
                // Ignore
            } finally {
                setHasTriedToLoadPreconfiguredMessages(true);
            }
        }
        loadPreconfiguredMessages();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [ show ]);

    const submit = useCallback(async () => {
        const logSessionEvent: Models.LogSessionEvent = {
            message: message
        };
        setIsSubmitting(true);
        await sendPostRequest(
            `api/log/${logSessionId}/events`, {}, 
            'Could not create event',
            logSessionEvent,
            async () => {
                showSuccessAlert('Successfully created event');
                onClose();
            },
            undefined,
            () => setIsSubmitting(false)
        )
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [logSessionId, message ]);

    return (<Modal
        show={show}
        onHide={onClose}
    >
        <Modal.Header closeButton>
            <Modal.Title>Create custom event</Modal.Title>
        </Modal.Header>
        <Modal.Body>
            <FormGroup>
                <FormLabel>Message</FormLabel>
                <FormControl
                    value={message}
                    onChange={e => setMessage(e.target.value)}
                    size="lg"
                />
                <div className="d-flex flex-column">
                {preconfiguredMessages.map(preconfiguredMessage => (
                    <Button
                        key={preconfiguredMessage}
                        onClick={() => setMessage(preconfiguredMessage)}
                        size="sm"
                        variant="outline-primary"
                        className="d-block my-1"
                    >
                        "{preconfiguredMessage}"
                    </Button>
                ))}
                </div>
            </FormGroup>
        </Modal.Body>
        <Modal.Footer>
            <Button
                variant="secondary"
                onClick={onClose}
                size="lg"
            >
                Cancel
            </Button>
            <AsyncButton
                onClick={submit}
                isExecuting={isSubmitting}
                activeText="Create"
                executingText="Creating..."
                size="lg"
            />
        </Modal.Footer>
    </Modal>);

}