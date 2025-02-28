export enum PatientMonitorType {
    Unknown = "Unknown",
    PhilipsIntellivue = "PhilipsIntellivue",
    GEDash = "GEDash",
    SimulatedPhilipsIntellivue = "SimulatedPhilipsIntellivue"
}
export enum Sex {
    Undefined = "Undefined",
    Female = "Female",
    Male = "Male",
    Other = "Other"
}
export enum MonitorDataType {
    Undefined = "Undefined",
    Numerics = "Numerics",
    Wave = "Wave"
}
export enum MeasurementState {
    VALID = "VALID",
    MSMT_STATE_AL_INHIBITED = "MSMT_STATE_AL_INHIBITED",
    MSMT_STATE_IN_ALARM = "MSMT_STATE_IN_ALARM",
    MSMT_ONGOING = "MSMT_ONGOING",
    EARLY_INDICATION = "EARLY_INDICATION",
    VALIDATED_DATA = "VALIDATED_DATA",
    DEMO_DATA = "DEMO_DATA",
    TEST_DATA = "TEST_DATA",
    CALIBRATION_ONGOING = "CALIBRATION_ONGOING",
    UNAVAILABLE = "UNAVAILABLE",
    QUESTIONABLE = "QUESTIONABLE",
    INVALID = "INVALID"
}
