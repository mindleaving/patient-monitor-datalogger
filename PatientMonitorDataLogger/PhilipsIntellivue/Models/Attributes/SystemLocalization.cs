using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class SystemLocalization : ISerializable
{
    public SystemLocalization(
        uint textCatalogRevision,
        Language language,
        StringFormat format)
    {
        TextCatalogRevision = textCatalogRevision;
        Language = language;
        Format = format;
    }

    public uint TextCatalogRevision { get; }
    public Language Language { get; }
    public StringFormat Format { get; }

    public static SystemLocalization Read(
        BigEndianBinaryReader binaryReader)
    {
        var textCatalogRevision = binaryReader.ReadUInt32();
        var language = (Language)binaryReader.ReadUInt16();
        var format = (StringFormat)binaryReader.ReadUInt16();
        return new(textCatalogRevision, language, format);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(TextCatalogRevision),
            ..BigEndianBitConverter.GetBytes((ushort)Language),
            ..BigEndianBitConverter.GetBytes((ushort)Format)
        ];
    }
}