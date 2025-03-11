using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest.PhilipsIntellivue.Models;

public class IntellivueStringTest
{
    [Test]
    public void HasExpectedLengthAndZeroByte()
    {
        var str = "Test";
        var sut = new IntellivueString(str);
        var bytes = sut.Serialize();

        var expectedLength = sizeof(ushort) + str.Length * 2 + 1; // Length + 2 bytes per character + '\0'
        Assert.That(bytes.Length, Is.EqualTo(expectedLength));
        Assert.That(bytes[expectedLength-1], Is.EqualTo((byte)0x00));
    }

    [Test]
    public void SerializationRoundtrip()
    {
        var str = "Test";
        var sut = new IntellivueString(str);
        var serialized = sut.Serialize();
        var reconstructed = IntellivueString.Read(new BigEndianBinaryReader(new MemoryStream(serialized)));

        Assert.That(reconstructed.Value, Is.EqualTo(sut.Value));
    }
}