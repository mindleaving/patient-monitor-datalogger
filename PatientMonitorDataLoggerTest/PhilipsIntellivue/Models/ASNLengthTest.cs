using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest.PhilipsIntellivue.Models;

public class ASNLengthTest
{
    [Test]
    [TestCase(0)]
    [TestCase(59)]
    [TestCase(127)]
    public void CanEncodeSmallNumber(int number)
    {
        var sut = new ASNLength(number);

        var bytes = sut.Serialize();
        Assert.That(bytes, Is.EqualTo(new[] { (byte)number }));
    }

    [Test]
    [TestCase(128, 2)]
    [TestCase(255, 2)]
    [TestCase(256, 3)]
    [TestCase(257, 3)] // Requires 2 bytes
    [TestCase(71681, 4)] // Requires 3 bytes
    public void CanEncodeLargeNumber(int number, int expectedByteCount)
    {
        var expectedNumberBytes = BigEndianBitConverter.GetBytes(number).SkipWhile(x => x == 0).ToArray();
        var sut = new ASNLength(number);

        var bytes = sut.Serialize();
        Assert.That(bytes.Length, Is.EqualTo(expectedByteCount));
        Assert.That(bytes[1..], Is.EqualTo(expectedNumberBytes));
    }

    [Test]
    [TestCase(0u)]
    [TestCase(127u)]
    [TestCase(128u)]
    [TestCase(255u)]
    [TestCase(256u)]
    [TestCase(257u)]
    [TestCase(71681u)]
    [TestCase((uint)int.MaxValue)]
    [TestCase(uint.MaxValue)]
    public void SerializationRoundtrip(uint number)
    {
        var sut = new ASNLength(number);
        var serialized = sut.Serialize();
        var deserialized = ASNLength.Read(new MemoryStream(serialized));

        Assert.That(deserialized.Length, Is.EqualTo(number));
    }
}