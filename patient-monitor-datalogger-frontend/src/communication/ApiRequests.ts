import { showErrorAlert } from "../helpers/AlertHelpers";
import { handleResponse } from "./ApiResponseHandler";
import { apiClient, QueryParameters } from "./ApiClient";

export const buildLoadObjectFunc = <T>(
    apiPath: string,
    params: QueryParameters = {},
    errorText: string,
    onItemLoaded: (item: T) => void,
    onFailure?: (response: Response | undefined) => Promise<void>,
    onFinally?: () => void) => {
    return async () => await loadObject(apiPath, params, errorText, onItemLoaded, onFailure, onFinally);
}
export const loadObject = async <T>(
    apiPath: string,
    params: QueryParameters = {},
    errorText: string,
    onItemLoaded: (item: T) => void,
    onFailure?: (response: Response | undefined) => Promise<void>,
    onFinally?: () => void
) => {
    const onSuccess = async (response: Response) => {
        const item = await response.json() as T;
        onItemLoaded(item);
    }
    try {
        const response = await apiClient.instance!.get(apiPath, params);
        await handleResponse(response, errorText, onSuccess, onFailure);
    } catch (error: any) {
        if(onFailure) {
            onFailure(undefined);
        }
        showErrorAlert(errorText, error.message);
    } finally {
        if(onFinally) {
            onFinally();
        }
    }
}

export const sendPostRequest = async (
    apiPath: string,
    params: QueryParameters,
    errorText: string,
    body: unknown,
    onSuccess?: (response: Response) => Promise<void>,
    onFailure?: (response: Response | undefined) => Promise<void>,
    onFinally?: () => void
) => {
    try {
        const response = await apiClient.instance!.post(apiPath, body, params);
        await handleResponse(response, errorText, onSuccess, onFailure);
    } catch(error: any) {
        if(onFailure) {
            onFailure(undefined);
        }
        showErrorAlert(errorText, error.message);
    } finally {
        if(onFinally) {
            onFinally();
        }
    }
}