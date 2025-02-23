using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class MdseUserInfoStd : IAssociationCommandUserData
{
    public MdseUserInfoStd(
        ProtocolVersion protocolVersion,
        NomenclatureVersion nomenclatureVersion,
        FunctionalUnits functionalUnits,
        SystemType systemType,
        StartupMode startupMode,
        List<AttributeValueAssertion> optionList,
        List<AttributeValueAssertion> supportedApplicationProfiles)
    {
        ProtocolVersion = protocolVersion;
        NomenclatureVersion = nomenclatureVersion;
        FunctionalUnits = functionalUnits;
        SystemType = systemType;
        StartupMode = startupMode;
        OptionList = optionList;
        SupportedApplicationProfiles = supportedApplicationProfiles;
    }

    public ProtocolVersion ProtocolVersion { get; }
    public NomenclatureVersion NomenclatureVersion { get; }
    public FunctionalUnits FunctionalUnits { get; }
    public SystemType SystemType { get; }
    public StartupMode StartupMode { get; }
    public List<AttributeValueAssertion> OptionList { get; }
    public List<AttributeValueAssertion> SupportedApplicationProfiles { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((uint)ProtocolVersion),
            ..BigEndianBitConverter.GetBytes((uint)NomenclatureVersion),
            ..BigEndianBitConverter.GetBytes((uint)FunctionalUnits),
            ..BigEndianBitConverter.GetBytes((uint)SystemType),
            ..BigEndianBitConverter.GetBytes((uint)StartupMode),
            ..OptionList.Serialize(),
            ..SupportedApplicationProfiles.Serialize()
        ];
    }

    public static MdseUserInfoStd Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var protocolVersion = (ProtocolVersion)binaryReader.ReadUInt32();
        var nomenclatureVersion = (NomenclatureVersion)binaryReader.ReadUInt32();
        var functionalUnits = (FunctionalUnits)binaryReader.ReadUInt32();
        var systemType = (SystemType)binaryReader.ReadUInt32();
        var startupMode = (StartupMode)binaryReader.ReadUInt32();
        var options = List<AttributeValueAssertion>.Read(binaryReader, x => AttributeValueAssertion.Read(x, context));
        var supportedApplicationProfiles = List<AttributeValueAssertion>.Read(binaryReader, x => AttributeValueAssertion.Read(x, context));
        return new(
            protocolVersion,
            nomenclatureVersion,
            functionalUnits,
            systemType,
            startupMode,
            options,
            supportedApplicationProfiles);
    }
}