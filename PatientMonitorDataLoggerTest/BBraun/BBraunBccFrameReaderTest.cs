using System.Text;
using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.Shared.Simulation;

namespace PatientMonitorDataLoggerTest.BBraun;

public class BBraunBccFrameReaderTest
{
    [Test]
    public void CanReadUnstuffedAdminAliveMessage()
    {
        var data = Encoding.UTF8.GetBytes(StringMessageHelpers.HumanFriendlyControlCharactersToRaw("<SOH>00031<STX>1/1/1>ADMIN:ALIVE<ETX>00061<EOT>"));
        var simulatedIoDevice = new SimulatedIoDevice();
        var settings = BBraunBccClientSettings.CreateForSimulatedConnection(BccParticipantRole.Server, false, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), simulatedIoDevice);
        simulatedIoDevice.Receive(data);
        using var sut = new BBraunBccFrameReader(simulatedIoDevice, settings);
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
        var data = Encoding.UTF8.GetBytes(StringMessageHelpers.HumanFriendlyControlCharactersToRaw("<SOH>00031<STX>1/1/1>ADMIN:ALIVE<ETX>00061<EOT>"));
        var stuffedData = BccCharacterStuffer.StuffCharacters(data);
        var simulatedIoDevice = new SimulatedIoDevice();
        var settings = BBraunBccClientSettings.CreateForSimulatedConnection(BccParticipantRole.Server, true, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), simulatedIoDevice);
        simulatedIoDevice.Receive(stuffedData);
        using var sut = new BBraunBccFrameReader(simulatedIoDevice, settings);
        BBraunBccFrame? received = null;
        sut.FrameAvailable += (_,frame) => received = frame;
        sut.Start();
        Assert.That(() => received, Is.Not.Null.After(500));
        Assert.That(received!.BedId, Is.EqualTo("1"));
        Assert.That(received!.Length, Is.EqualTo(31));
        Assert.That(received!.Checksum, Is.EqualTo(61));
    }

    
}