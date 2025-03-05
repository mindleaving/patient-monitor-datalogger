﻿namespace PatientMonitorDataLogger.BBraun.Models;

public class RackParameters
{
    public int NumberOfConnectedPillars { get; set; }
    public string? LastScannedBarcode { get; set; }
    public BBraunRackConfiguration RackConfiguration { get; set; }
    public ulong SerialNumber { get; set; }
    public string SoftwareVersion { get; set; }
}

public class BBraunRackConfiguration
{
    public int RackCountPillar1 { get; set; }
    public int RackCountPillar2 { get; set; }
    public int RackCountPillar3 { get; set; }
}

public class InfusionPumpParameters
{
    public string? Name { get; set; }
    public string? Model { get; set; }
    public double? RateInMilliliterPerHour { get; set; }
    public double? VolumeToBeInfusedInMilliliter { get; set; }
    public string? MedicationLongName { get; set; }
    public string? MedicationShortName { get; set; }
    public string? MedicationId { get; set; }
    public double? SizeOfSyringeInMilliliter { get; set; }
    public double? RemainingVolumeInSyringeInMilliliter { get; set; }
    public ushort? RemainingTimeOrNextPreAlarmInSeconds { get; set; }
    public double? VolumeInfusedInMilliliter { get; set; }
    public double? InfusionTimeInMinutes { get; set; }
    public double? BatteryTimeInMinutes { get; set; }
    public double? RemainingStandbyTimeInMinutes { get; set; }
    public double? RateOfBolusInMilliliterPerHour { get; set; }
    public double? BolusVolumeDeliveredInMilliliter { get; set; }
    public bool? IsDoseModeActive { get; set; }
    public double? DrugConcentration { get; set; }
    public byte? DrugConcentrationUnit { get; set; }
    public double? DoseRate { get; set; }
    public byte? DoseRateUnit { get; set; }
    public byte? ActualPressureSetting { get; set; }
    public byte? ActualPressureInPercentOfMax { get; set; }
    public bool? IsReadyForInfusion { get; set; }
    public bool? IsBolusActive { get; set; }
    public bool? IsActive { get; set; }
    public bool? UsesCCFunction { get; set; }
    public bool? IsBolusFunctionReleased { get; set; }
    public bool? IsInStandby { get; set; }
    public bool? IsDataLockOn { get; set; }
    public BccDevicePowerSource? PowerSource { get; set; }
}

public enum BccDevicePowerSource : byte
{
    Accu = 0,
    External = 1
}

public enum BBraunUnits : byte
{
    MilliliterPerHour = 1,
    Milliliter = 2,
    HoursMinutes = 3,
    MinutesSeconds = 4,
    Years = 5,
    Millimeter = 6,
    Nanoliter = 7,
    Degree = 8,
    Newton = 9,
    Millibar = 10,
    SquareMillimeter = 11,
    MilliAmpere = 12,
    Milliseconds = 13,
    Unixtime = 14,
    Percent = 15,
    MilliVolt = 16,

}

public static class BBraunUnitMap
{
    public static readonly Dictionary<int, string> Map = new()
    {
        { 1, "ml/h" },
        { 2, "ml" },
        { 3, "hh:mm" },
        { 4, "min:sec" },
        { 5, "Years" },
        { 6, "mm" },
        { 7, "nl" },
        { 8, "degree (angle)" },
        { 9, "N" },
        { 10, "mbar" },
        { 11, "square mm" },
        { 12, "mA" },
        { 13, "ms" },
        { 14, "Unixtime" },
        { 15, "Percent" },
        { 16, "mV" },
        { 17, "degree Celsius" },
        { 18, "mAh" },
        { 19, "m" },
        { 20, "BMI acc. to DUBOIS" },
        { 21, "BMI acc. to Boyd" },
        { 22, "kg" },
        { 23, "ng" },
        { 24, "mg" },
        { 25, "micro gram (µg)" },
        { 26, "g" },
        { 27, "mmol" },
        { 28, "mEq" },
        { 29, "IU" },
        { 30, "ng/ml" },
        { 31, "mg/ml" },
        { 32, "ug/ml" },
        { 33, "g/ml" },
        { 34, "mmol/ml" },
        { 35, "mEq/ml" },
        { 36, "IE/ml" },
        { 37, "ng/kg" },
        { 38, "mg/kg" },
        { 39, "micro gram/kg" },
        { 40, "g/kg" },
        { 41, "mmol/kg" },
        { 42, "mEq/kg" },
        { 43, "IU/kg" },
        { 44, "ng/minute" },
        { 45, "ng/hour" },
        { 46, "ng/day" },
        { 47, "mg/minute" },
        { 48, "mg/ hour" },
        { 49, "mg/day" },
        { 50, "micro gram/minute" },
        { 51, "micro gram/hour" },
        { 52, "micro gram/day" },
        { 53, "gram/minute" },
        { 54, "gram/hour" },
        { 55, "gram/day" },
        { 56, "mmol/minute" },
        { 57, "mmol/hour" },
        { 58, "mmol/day" },
        { 59, "mEq/minute" },
        { 60, "mEq/hour" },
        { 61, "mEq/day" },
        { 62, "IU/minute" },
        { 63, "IU/hour" },
        { 64, "IU/day" },
        { 65, "ng/kg/minute" },
        { 66, "ng/kg/ hour" },
        { 67, "ng/kg/day" },
        { 68, "mg/kg/minute" },
        { 69, "mg/kg/ hour" },
        { 70, "mg/kg/day" },
        { 71, "micro gram/kg/minute" },
        { 72, "micro gram/kg/hour" },
        { 73, "micro gram/kg/day" },
        { 74, "gram/kg/minute" },
        { 75, "gram/kg/hour" },
        { 76, "gram/kg/day" },
        { 77, "mmol/kg/minute" },
        { 78, "mmol/kg/hour" },
        { 79, "mmol/kg/day " },
        { 80, "mEq/kg/minute" },
        { 81, "mEq/kg/ hour" },
        { 82, "mEq/kg/day" },
        { 83, "IU/kg/minute" },
        { 84, "IU/kg/ hour" },
        { 85, "IU/kg/day" },
        { 86, "mmHg" },
        { 87, "kPa" },
        { 88, "lbs" },
        { 89, "mmol/liter" },
        { 90, "mg/dl" },
        { 91, "g(CH)" },
        { 92, "g(CH)" },
        { 93, "g(CH)" },
        { 94, "hour" },
        { 95, "cm" },
        { 96, "inch" },
        { 97, "ng/m^2" },
        { 98, "ng/m^2/minute" },
        { 99, "ng/m^2/hour" },
        { 100, "ng/m^2/day" },
        { 101, "mcg/m^2" },
        { 102, "mcg/m^2/minute" },
        { 103, "mcg/m^2/hour" },
        { 104, "mcg/m^2/day" },
        { 105, "mg/m^2" },
        { 106, "mg/m^2/minute" },
        { 107, "mg/m^2/hour" },
        { 108, "mg/m^2/day" },
        { 109, "g/m^2" },
        { 110, "g/m^2/minute" },
        { 111, "g/m^2/hour" },
        { 112, "g/m^2/day" },
        { 113, "IU/m^2" },
        { 114, "IU/m^2/minute" },
        { 115, "IU/m^2/hour" },
        { 116, "IU/m^2/day" },
        { 117, "mIU" },
        { 118, "mIU/ml" },
        { 119, "mIU/kg" },
        { 120, "mIU/m^2" },
        { 121, "mIU/minute" },
        { 122, "mIU/hour" },
        { 123, "mIU/day" },
        { 124, "mIU/kg/min" },
        { 125, "mIU/kg/hour " },
        { 126, "mIU//kg/day" },
        { 127, "mIU/m^2/minute" },
        { 128, "mIU/m^2/hour" },
        { 129, "mIU/m^2/day" },
        { 130, "kIU [kilo/thousand]" },
        { 131, "kIU/ml" },
        { 132, "kIU/kg" },
        { 133, "kIU/hour" },
        { 134, "kIU/day" },
        { 135, "kIU/kg/hour" },
        { 136, "kIU/kg/day" },
        { 137, "MIU [MEGA/Million]" },
        { 138, "MIU/ml" },
        { 139, "MIU/kg" },
        { 140, "MIU/hour" },
        { 141, "MIU/day" },
        { 142, "MIU/kg/hour" },
        { 143, "MIU/kg/day" },
        { 144, "mEq/m^2" },
        { 145, "mEq/m^2/minute" },
        { 146, "mEq/m^2/hour" },
        { 147, "mEq/m^2/day" },
        { 148, "kcal" },
        { 149, "kcal/kg" },
        { 150, "kcal/ml" },
        { 151, "kcal/day" },
        { 152, "kcal/kg/day" },
        { 153, "ml/kg" },
        { 154, "ml/ml" },
        { 155, "ml/kg/hour" },
        { 156, "Mbps" }
    };
}