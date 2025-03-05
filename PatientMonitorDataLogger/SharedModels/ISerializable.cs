namespace PatientMonitorDataLogger.SharedModels;

public interface ISerializable
{
    byte[] Serialize();
}