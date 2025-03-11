using System.Text;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class BBraunBccRequest : IBBraunBccMessage
{
    public const char Separator = ':';

    public BBraunBccRequest(
        string area,
        string command)
    {
        Area = area;
        Command = command;
    }

    public string Area { get; set; }
    public string Command { get; }

    public static BBraunBccRequest Read(
        byte[] bytes)
    {
        var str = Encoding.UTF8.GetString(bytes);
        var splitted = str.Split(Separator);
        if (splitted.Length != 2)
            throw new FormatException("Invalid format for BCC request");
        return new BBraunBccRequest(splitted[0], splitted[1]);
    }

    public byte[] Serialize()
    {
        return Encoding.UTF8.GetBytes(Area + Separator + Command);
    }
}