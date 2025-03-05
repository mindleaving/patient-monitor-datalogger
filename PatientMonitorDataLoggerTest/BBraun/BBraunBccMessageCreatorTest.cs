using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;
using System.Text;

namespace PatientMonitorDataLoggerTest.BBraun;

public class BBraunBccMessageCreatorTest
{
    [Test]
    public void AdminAliveMessageCorrect()
    {
        var expected = Encoding.UTF8.GetBytes(StringMessageHelpers.ReplaceControlCharacters("<SOH>00031<STX>1/1/1>ADMIN:ALIVE<ETX>00061<EOT>"));
        var settings = new BBraunBccClientSettings { UseCharacterStuffing = false };
        var sut = new BBraunBccMessageCreator(settings);

        var actual = sut.CreateAdminAliveMessage();

        Assert.That(actual, Is.EqualTo(expected));
    }
}