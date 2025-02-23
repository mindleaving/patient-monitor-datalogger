import { ApiClient, apiClient } from "./communication/ApiClient";

export const initializeApp = () => {
    apiClient.instance = window.location.port === "5173"
        ? new ApiClient(window.location.hostname, 5001, '/')
        : new ApiClient(window.location.hostname, 80, '/');
}