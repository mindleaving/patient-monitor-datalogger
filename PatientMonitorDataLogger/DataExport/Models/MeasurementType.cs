namespace PatientMonitorDataLogger.DataExport.Models;

public enum MeasurementType
{
    Undefined = 0, // For validation only. Do not use.
    HeartRateEcg = 1,
    HeartRateSpO2 = 2,

    SpO2 = 11,
    SpO2preDuctal = 12,

    RespirationRate = 21,


    EcgLeadI = 1001,
    EcgLeadII = 1002,
    EcgLeadIII = 1003,
    EcgLeadavR = 1004,
    EcgLeadavL = 1005,
    EcgLeadavF = 1006,
    EcgLeadV1 = 1007,
    EcgLeadV2 = 1008,
    EcgLeadV3 = 1009,
    EcgLeadV4 = 1010,
    EcgLeadV5 = 1011,
    EcgLeadV6 = 1012,
    EcgLeadV7 = 1013,
    EcgLeadV8 = 1014,
    EcgLeadV9 = 1015,
}