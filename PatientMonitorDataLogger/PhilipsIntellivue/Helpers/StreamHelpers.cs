namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public static class StreamHelpers
{
    public static void ReadExactLengthOrThrow(
        this Stream stream,
        byte[] buffer,
        int startIndex,
        int count)
    {
        if (buffer.Length < startIndex + count)
            throw new ArgumentOutOfRangeException(nameof(count), "Count + Start-Index cannot be greater than the length of the buffer");
        var bytesRead = stream.Read(buffer, startIndex, count);
        if (bytesRead != count)
            throw new EndOfStreamException();
    }
}