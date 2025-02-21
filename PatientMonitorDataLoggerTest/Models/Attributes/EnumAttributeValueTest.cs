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
}