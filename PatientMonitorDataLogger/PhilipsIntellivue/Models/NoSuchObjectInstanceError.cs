using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class NoSuchObjectInstanceError : IRemoteOperationErrorData
{
    public NoSuchObjectInstanceError(
        ManagedObjectId managedObject)
    {
        ManagedObject = managedObject;
    }

    public ManagedObjectId ManagedObject { get; }

    public static NoSuchObjectInstanceError Read(
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