using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class AssociationRequestUserData : IAssociationCommandUserData
{
    public AssociationRequestUserData(
        ASNLength length,
        MdseUserInfoStd userInfo)
    {
        Length = length;
        UserInfo = userInfo;
    }

    public ASNLength Length { get; }
    public MdseUserInfoStd UserInfo { get; }

    public static AssociationRequestUserData Read(
        BigEndianBinaryReader binaryReader)
    {
        var length = ASNLength.Read(binaryReader);
        var userInfo = MdseUserInfoStd.Read(binaryReader);
        return new(length, userInfo);
    }

    public byte[] Serialize()
    {
        return [..Length.Serialize(), ..UserInfo.Serialize()];
    }
}