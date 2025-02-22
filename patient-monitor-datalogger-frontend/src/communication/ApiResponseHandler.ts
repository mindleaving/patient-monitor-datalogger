import { showErrorAlert } from "../helpers/AlertHelpers";

export const handleResponse = async (
    response: Response,
    errorText: string,
    onSuccess?: (response: Response) => Promise<void>,
    onFailure?: (response: Response | undefined) => Promise<void>) => {

    if(response.ok) {
        if(onSuccess) {
            await onSuccess(response);
        }
    } else {
        if(onFailure) {
            await onFailure(response);
        } else {
            try {
                const errorDescription = await response.text();
                showErrorAlert(errorText, errorDescription);
            } catch {
                showErrorAlert(errorText);
            }
        }
    }
}