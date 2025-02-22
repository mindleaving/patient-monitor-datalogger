import { ApiClient, apiClient } from "./communication/ApiClient";

export const initializeApp = () => {
    apiClient.instance = window.location.hostname.toLowerCase() === "localhost"
        ? new ApiClient(window.location.hostname, 44301, '/')
        : new ApiClient(window.location.hostname, 443, '/');
}