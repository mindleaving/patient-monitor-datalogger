using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;

var settings = BBraunBccClientSettings.CreateForPhysicalConnection(BccParticipantRole.Client, "161.42.18.245", 4001, false, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
var tcpClient = new PhysicalTcpClient(settings.SpaceStationIp, settings.SpaceStationPort);
tcpClient.Open();
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