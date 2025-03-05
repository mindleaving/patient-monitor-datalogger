using System.Text;
using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;

namespace PatientMonitorDataLoggerTest.BBraun;

public class BBraunBccFrameReaderTest
{
    [Test]
    public void CanReadUnstuffedAdminAliveMessage()
    {
        var data = Encoding.UTF8.GetBytes(StringMessageHelpers.ReplaceControlCharacters("<SOH>00031<STX>1/1/1>ADMIN:ALIVE<ETX>00061<EOT>"));
        var settings = new BBraunBccClientSettings { UseCharacterStuffing = false };
        using var sut = new BBraunBccFrameReader(new MemoryStream(data), settings);
        BBraunBccFrame? received = null;
        sut.FrameAvailable += (_,frame) => received = frame;
        sut.Start();
        Assert.That(() => received, Is.Not.Null.After(500));
        Assert.That(received!.BedId, Is.EqualTo("1"));
        Assert.That(received!.Length, Is.EqualTo(31));
        Assert.That(received!.Checksum, Is.EqualTo(61));
    }

    [Test]
    public void CanReadStuffedAdminAliveMessage()
    {
        var data = Encoding.UTF8.GetBytes(StringMessageHelpers.ReplaceControlCharacters("<SOH>00031<STX>1/1/1>ADMIN:ALIVE<ETX>00061<EOT>"));
        var stuffedData = BccCharacterStuffer.StuffCharacters(data);
        var settings = new BBraunBccClientSettings { UseCharacterStuffing = true };
        using var sut = new BBraunBccFrameReader(new MemoryStream(stuffedData), settings);
        BBraunBccFrame? received = null;
        sut.FrameAvailable += (_,frame) => received = frame;
        sut.Start();
        Assert.That(() => received, Is.Not.Null.After(500));
        Assert.That(received!.BedId, Is.EqualTo("1"));
        Assert.That(received!.Length, Is.EqualTo(31));
        Assert.That(received!.Checksum, Is.EqualTo(61));
    }

    
}