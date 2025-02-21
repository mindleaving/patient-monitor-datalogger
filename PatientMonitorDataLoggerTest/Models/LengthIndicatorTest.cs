using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest.Models;

public class LengthIndicatorTest
{
    [Test]
    [TestCase((ushort)0)]
    [TestCase((ushort)0xfe)]
    public void CanEncodeSmallNumbers(ushort number)
    {
        var sut = new LengthIndicator(number);
        var bytes = sut.Serialize();

        Assert.That(bytes.Length, Is.EqualTo(1));
        Assert.That(bytes[0], Is.EqualTo(number));
    }

    [Test]
    [TestCase((ushort)0xff)]
    [TestCase((ushort)0x1886)]
    [TestCase(ushort.MaxValue)]
    public void CanEncodeLargeNumbers(ushort number)
    {
        var sut = new LengthIndicator(number);
        var bytes = sut.Serialize();

        Assert.That(bytes.Length, Is.EqualTo(3));
        Assert.That(bytes[0], Is.EqualTo(0xff));
    }

    [Test]
    [TestCase((ushort)0)]
    [TestCase((ushort)1)]
    [TestCase((ushort)255)]
    [TestCase((ushort)256)]
    [TestCase(ushort.MaxValue)]
    public void SerializationRoundtrip(ushort number)
    {
        var sut = new LengthIndicator(number);
        var serialized = sut.Serialize();
        var deserialized = LengthIndicator.Read(new MemoryStream(serialized));

        Assert.That(deserialized.Length, Is.EqualTo(number));
    }
}