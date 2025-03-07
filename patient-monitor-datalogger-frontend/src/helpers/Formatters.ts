import { InfusionPumpType, MedicalDeviceType, PatientMonitorType, WaveType } from "../types/enums";

export const deviceTypeNames: { [key:string]: string } = {
    [MedicalDeviceType.PatientMonitor]: "Patient Monitor",
    [MedicalDeviceType.InfusionPumps]: "Infusion pumps"
};

export const monitorNames: { [key:string]: string } = {
    [PatientMonitorType.PhilipsIntellivue]: "Philips Intellivue",
    [PatientMonitorType.SimulatedPhilipsIntellivue]: "Simulated Philips Intellivue",
    [PatientMonitorType.GEDash]: "GE Dash",
};

export const infusionPumpSystemNames: { [key:string]: string } = {
    [InfusionPumpType.BBraunSpace]: "B. Braun Space Infusion System"
};

export const waveTypeNames: { [waveType:string]: string } = {
    [WaveType.EcgDefault]: "Default ECG",
    [WaveType.EcgI]: "ECG Lead I",
    [WaveType.EcgII]: "ECG Lead II",
    [WaveType.EcgIII]: "ECG Lead III",
    [WaveType.EcgV1]: "ECG Lead V1",
    [WaveType.EcgV2]: "ECG Lead V2",
    [WaveType.EcgV3]: "ECG Lead V3",
    [WaveType.EcgV4]: "ECG Lead V4",
    [WaveType.EcgV5]: "ECG Lead V5",
    [WaveType.EcgV6]: "ECG Lead V6",
    [WaveType.Pleth]: "Pleth",
    [WaveType.Pleth2]: "Pleth 2",
    [WaveType.ArterialBloodPressure]: "Arterial Blood Pressure",
    [WaveType.Respiration]: "Respiration",
    [WaveType.CO2]: "CO2",
}
export const formatDate = (dateOrString: Date | string) => {
    const date = new Date(dateOrString);
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    const hours = date.getHours();
    const minutes = date.getMinutes();
    const seconds = date.getSeconds();
    return `${padLeft(year, 4, '0')}-${padLeft(month, 2, '0')}-${padLeft(day, 2, '0')} ${padLeft(hours, 2, '0')}:${padLeft(minutes, 2, '0')}:${padLeft(seconds, 2, '0')}`;
}
const padLeft = (number: number, length: number, padding: string) => {
    if(padding.length !== 1) {
        throw new Error("Padding must be one character");
    }
    return (number + '').padStart(length, padding);
}