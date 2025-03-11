namespace PatientMonitorDataLogger.BBraun.Helpers;

public static class BccCharacterStuffer
{
    public static byte[] StuffCharacters(
        byte[] input)
    {
        var d = (byte)'d';
        var D = (byte)'D';
        var e = (byte)'e';
        var E = (byte)'E';
        var ex = "ex"u8.ToArray();
        var ee = "ee"u8.ToArray();
        var EX = "EX"u8.ToArray();
        var EE = "EE"u8.ToArray();
        var output = new List<byte>();
        foreach (var inputByte in input)
        {
            if (inputByte == d) output.AddRange(ex);
            else if(inputByte == D) output.AddRange(EX);
            else if(inputByte == e) output.AddRange(ee);
            else if(inputByte == E) output.AddRange(EE);
            else output.Add(inputByte);
        }

        return output.ToArray();
    }
}