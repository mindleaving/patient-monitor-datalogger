import { confirmAlert } from 'react-confirm-alert';
import { AsyncButton } from './AsyncButton';

interface DeleteButtonProps {
    isDeleting?: boolean;
    onClick: (isConfirmed?: boolean) => void;
    requireConfirm?: boolean;
    confirmDialogTitle?: string;
    confirmDialogMessage?: string;
    className?: string;
    size?: "xs" | "sm" | "lg";
    type?: "button" | "submit" | "reset";
}

export const DeleteButton = (props: DeleteButtonProps) => {

    if(props.requireConfirm && !(props.confirmDialogTitle && props.confirmDialogMessage)) {
        throw new Error("If delete confirmation is required, title and message for the confirmation dialog must be specified");
    }

    const onClick = () => {
        if(props.requireConfirm) {
            confirmDelete();
            return;
        }
        props.onClick(false);
    }

    const confirmDelete = () => {
        confirmAlert({
            title: props.confirmDialogTitle,
            message: props.confirmDialogMessage,
            closeOnClickOutside: true,
            buttons: [
                {
                    label: "No, cancel",
                    onClick: () => {}
                },
                {
                    label: "Yes, delete",
                    onClick: () => props.onClick(true)
                }
            ]
        });
    }

    if(props.size === "xs") {
        return (<i
            className='fa fa-trash red clickable'
            onClick={e => {
                e.stopPropagation();
                onClick();
            }}
        />);
    }

    return (
        <AsyncButton
            variant="danger"
            className={props.className ?? 'm-2'}
            activeText={<><i className='fa fa-trash' /> Delete</>}
            executingText={<><i className='fa fa-trash' /> Deleting...</>}
            isExecuting={props.isDeleting}
            onClick={e => {
                e.stopPropagation();
                onClick();
            }}
            size={props.size}
            type={props.type}
        />
    );

}