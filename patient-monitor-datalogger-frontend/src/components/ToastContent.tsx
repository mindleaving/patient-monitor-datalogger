interface ToastContentProps {
    title: string;
    message?: string;
}
export const ToastContent = (props: ToastContentProps) => {
    return (
    <div>
        <strong>{props.title}</strong>
        {props.message 
        ? <div>
            {props.message}
        </div> : null}
    </div>);
}