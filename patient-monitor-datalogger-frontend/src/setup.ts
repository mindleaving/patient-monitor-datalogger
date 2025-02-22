import { ApiClient, apiClient } from "./communication/ApiClient";

export const initializeApp = () => {
    apiClient.instance = new ApiClient(window.location.hostname, 80, '/');
}