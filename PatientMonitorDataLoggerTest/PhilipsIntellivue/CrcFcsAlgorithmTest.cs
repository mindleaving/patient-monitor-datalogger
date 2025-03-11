using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLoggerTest.PhilipsIntellivue;

public class CrcFcsAlgorithmTest
{
    [Test]
    public void Roundtrip()
    {
        var rng = new Random();
        var data = new byte[35];
        rng.NextBytes(data);

        var checksum = CrcCcittFcsAlgorithm.CalculateFcs(data);

        byte[] combined = [..data, ..BitConverter.GetBytes((ushort)~checksum) ];
        var selfCheck = CrcCcittFcsAlgorithm.CalculateFcs(combined);

        Assert.That(selfCheck, Is.EqualTo(CrcCcittFcsAlgorithm.GoodFcs));
    }
}