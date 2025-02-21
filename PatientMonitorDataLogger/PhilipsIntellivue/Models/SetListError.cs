using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SetListError : IRemoteOperationErrorData
{
    public SetListError(
        ManagedObjectId managedObject,
        List<SetError> errors)
    {
        ManagedObject = managedObject;
        Errors = errors;
    }

    public ManagedObjectId ManagedObject { get; }
    public List<SetError> Errors { get; }

    public static SetListError Read(
        BigEndianBinaryReader binaryReader)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var setInfoList = List<SetError>.Read(binaryReader, SetError.Read);
        return new(managedObject, setInfoList);
    }

    public byte[] Serialize()
    {
        return
        [
            ..ManagedObject.Serialize(),
            ..Errors.Serialize()
        ];
    }
}