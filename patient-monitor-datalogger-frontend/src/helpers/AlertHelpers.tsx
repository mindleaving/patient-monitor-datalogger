import { confirmAlert } from "react-confirm-alert";
import { toast } from "react-toastify";

export const openConfirmDeleteAlert = (
    nameOfObjectToBeDeleted: string, 
    title: string, 
    message: string, 
    yesCallback: () => void, 
    noCallback: () => void = () => {}) => {        
    confirmAlert({
        title: title,
        message: message.replace('{0}', nameOfObjectToBeDeleted),
        closeOnClickOutside: true,
        buttons: [
            {
                label: "No, cancel",
                onClick: noCallback
            }, 
            {
                label: "Yes, delete",
                onClick: yesCallback
            }
        ]
    });
}
export const showSuccessAlert = (title: string, message?: string, durationInMs?: number) => {
    toast.success(<ToastContent title={title} message={message} />, {
        autoClose: durationInMs
    });
}
export const showWarningAlert = (title: string, message?: string, durationInMs?: number) => {
    toast.warning(<ToastContent title={title} message={message} />, {
        autoClose: durationInMs
    });
}
export const showErrorAlert = (title: string, message?: string, durationInMs?: number) => {
    toast.error(<ToastContent title={title} message={message} />, {
        autoClose: durationInMs
    });
}
export const showInfoAlert = (title: string, message?: string, durationInMs?: number) => {
    toast.info(<ToastContent title={title} message={message} />, {
        autoClose: durationInMs
    });
}

export interface ToastContentProps {
    title: string;
    message?: string;
}
export const ToastContent = (props: ToastContentProps) => {
    return (
    <>
        <strong>{props.title}</strong>
        {props.message 
        ? <div>
            {props.message}
        </div> : null}
    </>);
}