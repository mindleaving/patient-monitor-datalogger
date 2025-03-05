using System.Text;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class BBraunBccMessageCreator
{
    private readonly BBraunBccClientSettings settings;

    public BBraunBccMessageCreator(
        BBraunBccClientSettings settings)
    {
        this.settings = settings;
    }

    public byte[] CreateAcknowledgeMessage() => [0x06];
    public byte[] CreateNotAcknowledgeMessage() => [0x15];
    public byte[] CreateAdminAliveMessage() => CreateMessage("1", "ADMIN:ALIVE");
    public byte[] CreateGetAllMessage(string bedId) => CreateMessage(bedId, "MEM:GET");

    public byte[] CreateMessage(
        string bedId,
        string message)
    {
        var length = 1 + 5 + 1 + bedId.Length + 5 + message.Length + 1 + 5 + 1; // <SOH> + LENGTH:00000 + <SOT> + BEDID + "1/1>" + message + ETX/ETB + CHECKSUM:00000 + EOT
        var frame = new BBraunBccFrame(length, bedId, new GenericIbBraunBccMessage(message), 0);
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