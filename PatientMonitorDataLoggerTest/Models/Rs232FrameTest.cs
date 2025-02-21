using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PatientMonitorDataLogger.PhilipsIntellivue;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest.Models;

public class Rs232FrameTest
{
    [Test]
    public void SerializationRoundtrip()
    {
        var messageCreator = new CommandMessageCreator();
        var userData = messageCreator.CreateAssociationRequest(TimeSpan.FromSeconds(1), ExtendedPollProfileOptions.POLL_EXT_PERIOD_RTSA | ExtendedPollProfileOptions.POLL_EXT_PERIOD_NU_1SEC);
        var userDataBytes = userData.Serialize();
        var header = new Rs232FrameHeader(ProtocolId.DataExport, MessageType.AssociationControlOrDataExportCommand, (ushort)userDataBytes.Length);
        var headerBytes = header.Serialize();
        var checksum = CrcCcittFcsAlgorithm.CalculateFcs([..headerBytes, ..userDataBytes]);
        var frame = new Rs232Frame(header, userData, checksum);

        var serialized = frame.Serialize();
        var unescapedData = Unescape(serialized);
        var reconstructed = Rs232Frame.Parse(unescapedData);

        Assert.That(reconstructed.Header.UserDataLength, Is.EqualTo(frame.Header.UserDataLength));
        Assert.That(reconstructed.Checksum, Is.EqualTo(frame.Checksum));
        Assert.That(reconstructed.UserData, Is.TypeOf<AssociationCommandMessage>());
        var reconstructedAssociationCommandMessage = (AssociationCommandMessage)reconstructed.UserData;
        Assert.That(reconstructedAssociationCommandMessage.SessionHeader.Type, Is.EqualTo(AssociationCommandType.RequestAssociation));
        Assert.That(reconstructedAssociationCommandMessage.UserData, Is.TypeOf<AssociationRequestUserData>());
        var reconstructedAssociationRequest = (AssociationRequestUserData)reconstructedAssociationCommandMessage.UserData!;
        Assert.That(reconstructedAssociationRequest.UserInfo.SystemType, Is.EqualTo(((AssociationRequestUserData)userData.UserData!).UserInfo.SystemType));
        Assert.That(reconstructedAssociationRequest.UserInfo.StartupMode, Is.EqualTo(((AssociationRequestUserData)userData.UserData!).UserInfo.StartupMode));
    }

    [Test]
    [TestCase(@"")]
    public void CanReadFramesFromFile(
        string inputFilePath)
    {
        var input = File.ReadAllBytes(inputFilePath);
        var simulatedSerialPort = new SimulatedSerialPort();
        simulatedSerialPort.Receive(input);
        var sut = new PhilipsIntellivueSerialDataFrameReader(simulatedSerialPort);

        var messages = new System.Collections.Generic.List<ICommandMessage>();
        void OnFrameAvailable(
            object? sender,
            Rs232Frame e)
        {
            messages.Add(e.UserData);
        }
        sut.FrameAvailable += OnFrameAvailable;
        sut.Start();
        while (simulatedSerialPort.IncomingBytesCount > 0)
        {
            Task.Delay(100);
        }

        var jsonSerializerSettings = new JsonSerializerSettings
        {
            Converters = { new StringEnumConverter() }
        };
        File.WriteAllLines(@"C:\Temp\PhilpsIntellivue-MX800-message.json", messages.Select(message => JsonConvert.SerializeObject(message, jsonSerializerSettings)));
        Assert.That(messages.Count, Is.GreaterThan(0));
    }

    private static byte[] Unescape(
        byte[] serialized)
    {
        using var memoryStream = new MemoryStream(serialized);
        using var escapedStream = new TransparencyByteUnescapedStream(memoryStream);
        var buffer = new byte[serialized.Length];
        var bytesRead = escapedStream.Read(buffer);
        var unescapedData = buffer[..bytesRead];
        return unescapedData;
    }
}