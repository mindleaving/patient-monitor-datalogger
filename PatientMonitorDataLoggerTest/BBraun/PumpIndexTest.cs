using PatientMonitorDataLogger.BBraun.Models;

namespace PatientMonitorDataLoggerTest.BBraun;

public class PumpIndexTest
{
    [Test]
    [TestCase("50100", 0, 0)]
    [TestCase("50111", 1, 1)]
    [TestCase("50119", 1, 9)]
    [TestCase("5011B", 1, 11)]
    [TestCase("5011O", 1, 24)]
    [TestCase("501112", 1, 12)]
    public void CanParse(string str, int expectedPillarIndex, int expectedSlotIndex)
    {
        var sut = PumpIndex.Parse(str);
        Assert.That(sut.Pillar, Is.EqualTo(expectedPillarIndex));
        Assert.That(sut.Slot, Is.EqualTo(expectedSlotIndex));
    }
}