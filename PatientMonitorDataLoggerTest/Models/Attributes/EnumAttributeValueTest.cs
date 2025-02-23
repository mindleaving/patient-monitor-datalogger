using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

namespace PatientMonitorDataLoggerTest.Models.Attributes;

public class EnumAttributeValueTest
{
    [Test]
    public void CanDetermineEnumLengthFromType()
    {
        Assert.That(sizeof(PatientState) == sizeof(ushort));
        Assert.That(sizeof(PatientState) != sizeof(uint));
        Assert.That(sizeof(PatientState) != sizeof(byte));
    }

    [Test]
    public void CanSerialize()
    {
        var sut = new EnumAttributeValue<PatientState>(PatientState.ADMITTED, BigEndianBitConverter.GetBytes((ushort)PatientState.ADMITTED));
        byte[]? buffer = null;
        Assert.That(() => buffer = sut.Serialize(), Throws.Nothing);
        Assert.That(buffer, Is.Not.Null);
        Assert.That(buffer!.Length == sizeof(ushort));
    }
}