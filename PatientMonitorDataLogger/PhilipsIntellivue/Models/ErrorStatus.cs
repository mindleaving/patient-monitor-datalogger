namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public enum ErrorStatus : ushort
{
    ATTR_ACCESS_DENIED = 2,
    ATTR_NO_SUCH_ATTRIBUTE = 5,
    ATTR_INVALID_ATTRIBUTE_VALUE = 6,
    ATTR_INVALID_OPERATION = 24,
    ATTR_INVALID_OPERATOR = 25
}