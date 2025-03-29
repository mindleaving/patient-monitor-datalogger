import { useCallback, useEffect, useState } from "react";
import { Button, Modal, ModalTitle } from "react-bootstrap";
import { useNavigate } from "react-router";
import { buildLoadObjectFunc, sendPostRequest } from "../communication/ApiRequests";
import { confirmAlert } from 'react-confirm-alert';

interface MenuModalProps {
    show: boolean;
    onClose: () => void;
    isAnyLogSessionRunning: boolean;
}

export const MenuModal = (props: MenuModalProps) => {

    const { show, onClose, isAnyLogSessionRunning } = props;

    const navigate = useNavigate();

    const [ version, setVersion ] = useState<string>();

    useEffect(() => {
        if(version) {
            return;
        }
        const loadVersion =  buildLoadObjectFunc(
            'api/system/version', {},
            'Could not get system version',
            setVersion
        );
        loadVersion();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const shutdown = useCallback(async (force: boolean = false) => {
        onClose();
        if(!force) {
            confirmAlert({
                title: 'Shutdown?',
                message: 'Are you sure you want to shut down?',
                buttons: [
                    {
                        label: 'No, cancel'
                    },
                    {
                        label: 'Yes, shutdown',
                        onClick: () => shutdown(true)
                    }
                ]
            });
            return;
        }
        await sendPostRequest(
            'api/system/shutdown', {},
            'Could not shutdown. Is any session still running?',
            null
        );
    }, [ onClose ]);

    return (<Modal
        show={show}
        onHide={onClose}
    >
        <Modal.Header closeButton />
        <Modal.Body>
            <div className="d-flex flex-column">
                <Button
                    onClick={() => navigate('/create/session')}
                    size="lg"
                    className="my-2"
                >
                    + Create data log session
                </Button>
                <Button
                    onClick={() => shutdown()}
                    size="lg"
                    variant="danger"
                    className="my-2"
                    disabled={isAnyLogSessionRunning}
                >
                    {isAnyLogSessionRunning 
                    ? <span>Shutdown <small>(disabled: Log sessions running)</small></span>
                    : 'Shutdown'}
                </Button>
                {version
                ? <div className="text-center mt-3">
                    Version: {version}
                </div> : null}
            </div>
        </Modal.Body>
    </Modal>);

}