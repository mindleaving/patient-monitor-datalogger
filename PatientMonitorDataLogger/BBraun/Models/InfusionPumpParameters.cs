namespace PatientMonitorDataLogger.BBraun.Models;

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