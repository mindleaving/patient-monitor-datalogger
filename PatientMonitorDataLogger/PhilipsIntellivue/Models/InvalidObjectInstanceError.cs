using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class InvalidObjectInstanceError : IRemoteOperationErrorData
{
    public InvalidObjectInstanceError(
        ManagedObjectId managedObject)
    {
        ManagedObject = managedObject;
    }

    public ManagedObjectId ManagedObject { get; }

    public static InvalidObjectInstanceError Read(
        BigEndianBinaryReader binaryReader)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        return new(managedObject);
    }

    public byte[] Serialize()
    {
        return ManagedObject.Serialize();
    }
}