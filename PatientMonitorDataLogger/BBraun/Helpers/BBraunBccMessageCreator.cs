using System.Text;
using PatientMonitorDataLogger.BBraun.Models;
using PatientMonitorDataLogger.BBraun.Simulation;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class BBraunBccMessageCreator
{
    public const char AcknowledgeCharacter = '\x06';
    public const char NotAcknowledgeCharacter = '\x15';

    private readonly BBraunBccClientSettings settings;

    public BBraunBccMessageCreator(
        BBraunBccClientSettings settings)
    {
        this.settings = settings;
    }

    public byte[] CreateAcknowledgeMessage() => [(byte)AcknowledgeCharacter];
    public byte[] CreateNotAcknowledgeMessage() => [(byte)NotAcknowledgeCharacter];
    public byte[] CreateInitializeCommunicationRequest() => CreateMessage("1", new BBraunBccRequest("ADMIN", "ALIVE"));
    public byte[] CreateVersionRequest(string bedId) => CreateMessage(bedId, new BBraunBccRequest("ADMIN", "VERSION"));
    public byte[] CreateGetAllRequest(string bedId) => CreateMessage(bedId, new BBraunBccRequest("MEM", "GET"));
    public byte[] CreateGetPumpRequest(string bedId, PumpIndex pumpIndex) => CreateMessage(bedId, new BBraunBccRequest("MEM", $"GETSLOT#{pumpIndex.Pillar}{pumpIndex.SlotCharacter}"));
    public byte[] CreateResponseMessage(string bedId, List<Quadruple> quadruples) => CreateMessage(bedId, new BBraunBccResponse(quadruples));

    public byte[] CreateErrorResponse(
        string bedId,
        uint secondsSinceStart,
        PumpIndex pumpIndex,
        BccErrorCodes errorCode)
        => CreateResponseMessage(
            bedId,
            [
                new(
                    secondsSinceStart,
                    pumpIndex,
                    "GNERR",
                    ((int)errorCode).ToString())
            ]);

    public byte[] CreateMessage(
        string bedId,
        IBBraunBccMessage message)
    {
        var messageBytes = message.Serialize();
        var length = 1 + 5 + 1 + bedId.Length + 5 + messageBytes.Length + 1 + 5 + 1; // <SOH> + LENGTH:00000 + <SOT> + BEDID + "1/1>" + message + ETX/ETB + CHECKSUM:00000 + EOT
        var frame = new BBraunBccFrame(length, bedId, message, 0);
        var frameBytes = frame.Serialize();
        var checksum = BccChecksumCalculator.Calculate(frameBytes[..^6]);
        InsertChecksum(frameBytes, checksum);
        var unstuffedBytes = frameBytes;
        if (!settings.UseCharacterStuffing)
            return unstuffedBytes;
        return BccCharacterStuffer.StuffCharacters(unstuffedBytes);
    }

    private void InsertChecksum(
        byte[] frameBytes,
        int checksum)
    {
        var checksumString = checksum.ToString("00000");
        var checksumBytes = Encoding.ASCII.GetBytes(checksumString);
        for (int i = 0; i < 5; i++)
        {
            frameBytes[frameBytes.Length - 6 + i] = checksumBytes[i];
        }
    }
}