using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest.PhilipsIntellivue.Models;

public class IntellivueFloatTest
{
    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(-1)]
    [TestCase(0.43f)]
    [TestCase(-0.43f)]
    [TestCase(879168.284f)]
    [TestCase(-879168.284f)]
    
    public void SerializationRoundtrip(float number)
    {
        var sut = new IntellivueFloat(number);
        var serialized = sut.Serialize();
        var binaryReader = new BigEndianBinaryReader(new MemoryStream(serialized));
        var reconstructed = IntellivueFloat.Read(binaryReader);
        
        Assert.That(reconstructed.Value, Is.EqualTo(number).Within(1e-3 * Math.Abs(number)));
    }

    [Test]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void SerializationRoundtripForSpecialValues(float value)
    {
        var sut = new IntellivueFloat(value);
        var serialized = sut.Serialize();
        var binaryReader = new BigEndianBinaryReader(new MemoryStream(serialized));
        var reconstructed = IntellivueFloat.Read(binaryReader);

        Assert.That(reconstructed.Value, Is.EqualTo(value));
    }

    [Test]
    [TestCase(0xfd007d00u, 32.000f)]
    [TestCase(0xff000140u, 32.0f)]
    [TestCase(0x01000140u, 3200f)]
    [TestCase(0x02000020u, 3200f)]
    public void CanReadExamplesFromDevelopmentManual(uint input, float expected)
    {
        var binaryReader = new BigEndianBinaryReader(new MemoryStream(BigEndianBitConverter.GetBytes(input)));
        var sut = IntellivueFloat.Read(binaryReader);
        var actual = sut.Value;

        Assert.That(actual, Is.EqualTo(expected).Within(1e-5));
    }
}