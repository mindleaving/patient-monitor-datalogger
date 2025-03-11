using System.Text;
using System.Text.RegularExpressions;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class BBraunBccFrame : ISerializable
{
    public BBraunBccFrame(
        int length,
        string bedId,
        IBBraunBccMessage userData,
        int checksum)
    {
        Length = length;
        BedId = bedId;
        UserData = userData;
        Checksum = checksum;
    }

    public int Length { get; }
    public string BedId { get; }
    public IBBraunBccMessage UserData { get; }
    public int Checksum { get; set; }

    public static BBraunBccFrame Parse(
        byte[] buffer,
        BccParticipantRole role)
    {
        if(buffer[0] != BBraunBccFrameReader.StartOfHeaderCharacter)
            throw new FormatException("Didn't find start of head character");
        if (buffer[6] != BBraunBccFrameReader.StartOfTextCharacter)
            throw new FormatException("Didn't find start of text character");
        var lengthString = Encoding.ASCII.GetString(buffer[1..6]);
        var length = int.Parse(lengthString);
        if (buffer.Length != length)
            throw new ArgumentException("Length of buffer and length in message header do not agree");
        if (buffer[length - 1] != BBraunBccFrameReader.EndOfTransmissionCharacter)
            throw new FormatException("Didn't find end of transmission character");
        if (buffer[length - 7] != BBraunBccFrameReader.EndOfTextCharacter && buffer[length - 7] != BBraunBccFrameReader.EndOfTextBlockCharacter)
            throw new FormatException("Didn't find end of text/text block character");
        var checksumString = Encoding.ASCII.GetString(buffer[^6..^1]);
        var checksum = int.Parse(checksumString);
        if (!BccChecksumCalculator.IsChecksumCorrect(buffer[..^6], checksum))
            throw new FormatException("Corrupt data. Checksum doesn't match");
        var bedId = ExtractBedId(Encoding.ASCII.GetString(buffer[7..26]));
        var userDataBytes = buffer[(7+bedId.Length+5)..^7];
        var userData = ParseUserData(userDataBytes, role);
        return new BBraunBccFrame(
            length,
            bedId,
            userData,
            checksum);
    }

    private static IBBraunBccMessage ParseUserData(
        byte[] bytes,
        BccParticipantRole role)
    {
        return role switch
        {
            BccParticipantRole.Client => BBraunBccResponse.Read(bytes),
            BccParticipantRole.Server => BBraunBccRequest.Read(bytes),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }

    private static string ExtractBedId(
        string str)
    {
        var bedIdMatch = Regex.Match(str, "^(?<bedID>[\x2F-\x39\x41-\x7A]{1,15})/1/1>");
        if (!bedIdMatch.Success)
            throw new FormatException("Couldn't find bed-ID");
        return bedIdMatch.Groups["bedID"].Value;
    }

    /// <summary>
    /// Serialize frame WITHOUT character stuffing
    /// </summary>
    public byte[] Serialize()
    {
        return [
            BBraunBccFrameReader.StartOfHeaderCharacter,
            ..Encoding.ASCII.GetBytes(Length.ToString("00000")),
            BBraunBccFrameReader.StartOfTextCharacter,
            ..Encoding.ASCII.GetBytes($"{BedId}/1/1>"),
            ..UserData.Serialize(),
            BBraunBccFrameReader.EndOfTextCharacter,
            ..Encoding.ASCII.GetBytes(Checksum.ToString("00000")),
            BBraunBccFrameReader.EndOfTransmissionCharacter,
        ];
    }

    public override string ToString()
    {
        return StringMessageHelpers.RawControlCharactersToHumanFriendly(Serialize())
            .Replace("<STX>", "<STX>" + Environment.NewLine)
            .Replace("1/1>", "1/1>" + Environment.NewLine)
            .Replace("<RS>", "<RS>" + Environment.NewLine);
    }
}