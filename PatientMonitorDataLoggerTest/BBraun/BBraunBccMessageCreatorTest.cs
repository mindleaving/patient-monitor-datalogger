using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;
using System.Text;

namespace PatientMonitorDataLoggerTest.BBraun;

public class BBraunBccMessageCreatorTest
{
    [Test]
    public void AdminAliveMessageCorrect()
    {
        var expected = Encoding.UTF8.GetBytes(StringMessageHelpers.HumanFriendlyControlCharactersToRaw("<SOH>00031<STX>1/1/1>ADMIN:ALIVE<ETX>00061<EOT>"));
        var settings = BBraunBccClientSettings.CreateForPhysicalConnection(BccParticipantRole.Client, "192.168.100.41", 4001, false, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        var sut = new BBraunBccMessageCreator(settings);

        var actual = sut.CreateInitializeCommunicationRequest();

        Assert.That(actual, Is.EqualTo(expected));
    }
}