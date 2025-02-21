namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public enum DataExportCommandType : ushort
{
    EventReport = 0,
    ConfirmedEventReport = 1,
    Get = 3,
    Set = 4,
    ConfirmedSet = 5,
    ConfirmedAction = 7
}