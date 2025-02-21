namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public enum RemoteOperationErrorType : ushort
{
    NoSuchObjectClass = 0,
    NoSuchObjectInstance = 1,
    AccessDenied = 2,
    GetListError = 7,
    SetListError = 8,
    NoSuchAction = 9,
    ProcessingFailure = 10,
    InvalidArgumentValue = 15,
    InvalidScope = 16,
    InvalidObjectInstance = 17
}