namespace PatientMonitorDataLogger.Shared.Models;

public interface ISerializable
{
    byte[] Serialize();
}