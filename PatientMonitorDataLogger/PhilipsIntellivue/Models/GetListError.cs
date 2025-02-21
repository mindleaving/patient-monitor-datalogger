using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class GetListError : IRemoteOperationErrorData
{
    public GetListError(
        ManagedObjectId managedObject,
        List<GetError> errors)
    {
        ManagedObject = managedObject;
        Errors = errors;
    }

    public ManagedObjectId ManagedObject { get; }
    public List<GetError> Errors { get; }

    public static GetListError Read(
        BigEndianBinaryReader binaryReader)
    {
        var managedObject = ManagedObjectId.Read(binaryReader);
        var getInfoList = List<GetError>.Read(binaryReader, GetError.Read);
        return new(managedObject, getInfoList);
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