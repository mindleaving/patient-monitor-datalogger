import { useCallback, useEffect, useState } from "react";
import { FormControl, FormGroup, FormLabel, Modal } from "react-bootstrap";
import { Models } from "../types/models";
import { loadObject, sendPostRequest } from "../communication/ApiRequests";
import { AsyncButton } from "./AsyncButton";
import { showSuccessAlert } from "../helpers/AlertHelpers";
import { LoadingAlert } from "./LoadingAlert";

interface CopyToUsbModalProps {
    show: boolean;
    onClose: () => void;
    logSessionId: string;
}

export const CopyToUsbModal = (props: CopyToUsbModalProps) => {

    const { show, onClose, logSessionId } = props;

    const [ isLoadingUsbDrives, setIsLoadingUsbDrives ] = useState<boolean>(true);
    const [ availableUsbDrives, setAvailableUsbDrives ] = useState<Models.DataExport.UsbDriveInfo[]>([]);
    const [ selectedUsbDrive, setSelectedUsbDrive ] = useState<Models.DataExport.UsbDriveInfo>();
    const [ isCopying, setIsCopying ] = useState<boolean>(false);

    const loadAvailableUsbDrives = useCallback(async () => {
        setIsLoadingUsbDrives(true);
        await loadObject(
            'api/dataexport/usb-drives', {},
            "Could not load available USB drives",
            setAvailableUsbDrives,
            undefined,
            () => setIsLoadingUsbDrives(false)
        );
    }, []);

    useEffect(() => {
        if(!show) {
            return;
        }
        loadAvailableUsbDrives();
    }, [ show, loadAvailableUsbDrives ]);

    const copyDataToUsbDrive = useCallback(async () => {
        if(!selectedUsbDrive) {
            return;
        }
        setIsCopying(true);
        const body: Models.RequestBodies.CopyDataToUsbDriveRequest = {
            logSessionId: logSessionId,
            usbDrivePath: selectedUsbDrive.path
        }
        await sendPostRequest(
            'api/dataexport/to-usb', {},
            "Could not copy data to USB drive",
            body,
            async _ => {
                showSuccessAlert("Successfully copied data");
                onClose();
            },
            undefined,
            () => setIsCopying(false)
        );
    }, [ logSessionId, selectedUsbDrive, onClose ]);

    const onCloseIfNotCopying = () => {
        if(isCopying) {
            return;
        }
        onClose();
    }

    return (<Modal
        show={show}
        onHide={onCloseIfNotCopying}
        backdrop="static"
    >
        <Modal.Header closeButton>
            <Modal.Title>Copy data to </Modal.Title>
        </Modal.Header>
        <Modal.Body>
            {isLoadingUsbDrives
            ? <LoadingAlert />
            : <>
                <FormGroup>
                    <FormLabel>USB Drives</FormLabel>
                    <FormControl
                        as="select"
                        value={selectedUsbDrive?.path ?? ''}
                        onChange={e => {
                            const value = e.target.value;
                            if(!value) {
                                setSelectedUsbDrive(undefined);
                            } else {
                                setSelectedUsbDrive(availableUsbDrives.find(x => x.path === value));
                            }
                        }}
                    >
                        <option value="">Please select...</option>
                        {availableUsbDrives.map(usbDrive => (
                            <option key={usbDrive.path} value={usbDrive.path}>{usbDrive.path}: {usbDrive.name}</option>
                        ))}
                    </FormControl>
                </FormGroup>
            </>}
        </Modal.Body>
        <Modal.Footer>
            <AsyncButton
                onClick={copyDataToUsbDrive}
                isExecuting={isCopying}
                activeText="Copy"
                executingText="Copying..."
                disabled={!selectedUsbDrive}
                size="lg"
            />
        </Modal.Footer>
    </Modal>);

}