namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public enum RemoteOperationType : ushort
{
    Invoke = 1,
    Result = 2,
    Error = 3,
    LinkedResult = 5
}