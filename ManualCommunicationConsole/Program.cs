using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;

var tcpClient = new PhysicalTcpClient("192.168.100.41", 4001);
tcpClient.Open();
var settings = new BBraunBccClientSettings(BccParticipantRole.Client, false, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
var messageCreator = new BBraunBccMessageCreator(settings);
//var message = messageCreator.CreateResponseMessage("ComSystem", [ new(207, new(0,0), "V3.30", null) ]); //messageCreator.CreateGetPumpRequest("ComSystem", new PumpIndex(1, 3));
//var frame = BBraunBccFrame.Parse(message, BccParticipantRole.Client);
//Console.WriteLine(frame.ToString());
//return;
var protocolCommunicator = new BBraunBccCommunicator(tcpClient, settings, nameof(BBraunBccCommunicator));
var bedId = "1";
protocolCommunicator.NewMessage += OnNewFrameReceived;

protocolCommunicator.Start();
protocolCommunicator.Enqueue(messageCreator.CreateInitializeCommunicationRequest());
while (true)
{
    var command = Console.ReadLine();
    if(command != null && command.Trim().ToLower() == "q")
        break;
    protocolCommunicator.Enqueue(messageCreator.CreateGetAllRequest(bedId));
}

void OnNewFrameReceived(
    object? sender,
    BBraunBccFrame frame)
{
    Console.WriteLine($"Detected bed name '{frame.BedId}'");
    bedId = frame.BedId;
    File.AppendAllText("bbraun-bcc-messages.txt", frame.ToString().ReplaceLineEndings("") + Environment.NewLine);
}