import { Models } from "../../types/models";

interface SimulatedPhilipsIntellivueSettingsFormControlsProps {
    value: Models.SimulatedPhilipsIntellivueSettings;
    onChange: (update: Update<Models.IMedicalDeviceSettings>) => void;
}

export const SimulatedPhilipsIntellivueSettingsFormControls = (props: SimulatedPhilipsIntellivueSettingsFormControlsProps) => {

    const { value, onChange } = props;

    return (<></>);

}