using PatientMonitorDataLogger.BBraun.Models;
using PatientMonitorDataLogger.Shared.Helpers;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class BBraunBccResponse : IBBraunBccMessage
{
    public const char RecordSeparator = '\x1E';

    public BBraunBccResponse(
        List<Quadruple> quadruples)
    {
        Quadruples = quadruples;
    }

    public List<Quadruple> Quadruples { get; set; }

    public static BBraunBccResponse Read(
        byte[] bytes)
    {
        var records = bytes.Split((byte)RecordSeparator);
        var quadruples = records.Select(Quadruple.Read).ToList();
        return new(quadruples);
    }

    public byte[] Serialize()
    {
        return Quadruples.Select(quadruple => quadruple.Serialize()).Aggregate((a,b) => [ ..a, (byte)RecordSeparator, ..b ]);
    }
}