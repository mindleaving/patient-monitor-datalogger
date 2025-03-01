import { PatientMonitorType, WaveType } from "../types/enums";

export const monitorNames: { [key:string]: string } = {
    [PatientMonitorType.PhilipsIntellivue]: "Philips Intellivue",
    [PatientMonitorType.SimulatedPhilipsIntellivue]: "Simulated Philips Intellivue",
    [PatientMonitorType.GEDash]: "GE Dash",
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