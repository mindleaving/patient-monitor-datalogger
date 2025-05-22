namespace PatientMonitorDataLogger.Shared.Helpers;

public static class ExceptionExtensions
{
    public static Exception InnermostException(
        this Exception e)
    {
        var innerException = e;
        while (innerException.InnerException != null)
        {
            innerException = innerException.InnerException;
        }
        return innerException;
    }
}