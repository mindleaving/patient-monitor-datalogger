import { HubConnectionBuilder } from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { Badge } from "react-bootstrap";
import { apiClient } from "../communication/ApiClient";
import { showErrorAlert } from "../helpers/AlertHelpers";
import { Models } from "../types/models";

interface NumericsSignalRConnectionIndicatorProps {
    onNewObservationsAvailable: (data: Models.DataExport.LogSessionObservations) => void;
    onPatientInfoAvailable: (patientInfo: Models.PatientInfo) => void;
    onLogStatusChanged: (logStatus: Models.LogStatus) => void;
}

export const NumericsSignalRConnectionIndicator = (props: NumericsSignalRConnectionIndicatorProps) => {

    const { onNewObservationsAvailable: onNewNumericsDataAvailable, onPatientInfoAvailable, onLogStatusChanged } = props;

    const [ isConnecting, setIsConnecting ] = useState<boolean>(true);
    const [ isConnected, setIsConnected ] = useState<boolean>(false);

    useEffect(() => {
        setIsConnecting(true);
        const notificationsConnection = new HubConnectionBuilder()
            .withUrl(apiClient.instance!._buildUrl('hubs/data', {}))
            .withAutomaticReconnect()
            .build();
        const connectToHub = async () => {
            try {
                await notificationsConnection.start();
                notificationsConnection.on('ReceiveObservations', onNewNumericsDataAvailable);
                notificationsConnection.on('ReceivePatientInfo', onPatientInfoAvailable);
                notificationsConnection.on('ReceiveStatusChange', onLogStatusChanged);
                setIsConnected(true);
            } catch(error: any) {
                showErrorAlert("Could not subscribe to real-time numerics data", error.message);
                setIsConnected(false);
            } finally {
                setIsConnecting(false);
            }
        }
        connectToHub();
        return () => {
            notificationsConnection.stop();
        }
    }, []);

    return (<Badge pill 
        bg={isConnected ? 'success' : isConnecting ? 'warning' : 'danger'}
        text={isConnected ? 'light' : isConnecting ? 'dark' : 'light'}
    >
        {isConnected ? 'Connected' : isConnecting ? 'Connecting...' : 'Not connected'}
    </Badge>);

}