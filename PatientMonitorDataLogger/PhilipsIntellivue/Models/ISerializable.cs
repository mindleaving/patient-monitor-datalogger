namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public interface ISerializable
{
    byte[] Serialize();
}