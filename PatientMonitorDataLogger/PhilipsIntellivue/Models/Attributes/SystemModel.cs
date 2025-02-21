using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class SystemModel : ISerializable
{
    public SystemModel(
        byte[] manufacturer,
        byte[] modelNumber)
    {
        Manufacturer = manufacturer;
        ModelNumber = modelNumber;
    }

    public byte[] Manufacturer { get; }
    public byte[] ModelNumber { get; }

    public static SystemModel Read(
        BigEndianBinaryReader binaryReader)
    {
        var manufacturer = new byte[4];
        var bytesRead = binaryReader.Read(manufacturer);
        if (bytesRead != manufacturer.Length)
            throw new EndOfStreamException();
        var modelNumber = new byte[6];
        bytesRead = binaryReader.Read(modelNumber);
        if (bytesRead != modelNumber.Length)
            throw new EndOfStreamException();
        return new(manufacturer, modelNumber);
    }

    public byte[] Serialize()
    {
        return
        [
            ..Manufacturer,
            ..ModelNumber
        ];
    }
}