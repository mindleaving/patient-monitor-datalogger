import { Alert } from 'react-bootstrap';

interface NoEntriesAlertProps {}

export const NoEntriesAlert = (props: NoEntriesAlertProps) => {

    return (
        <Alert 
            variant="info" 
            className='text-center'
        >
            No entries
        </Alert>
    );

}