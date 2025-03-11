namespace PatientMonitorDataLogger.BBraun.Models;

public enum BccErrorCodes
{
    NoFurtherDataAvailableOrNoPumpsInSystem = 11,
    PumpWasRemoved = 39,
    WrongSyntax = 68,
    WrongPumpAddress = 811
}