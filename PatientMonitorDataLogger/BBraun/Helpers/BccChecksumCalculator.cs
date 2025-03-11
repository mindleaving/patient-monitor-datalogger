namespace PatientMonitorDataLogger.BBraun.Helpers;

public static class BccChecksumCalculator
{
    public static bool IsChecksumCorrect(
        byte[] bytes,
        int expectedChecksum)
    {
        var actualChecksum = Calculate(bytes);
        return actualChecksum == expectedChecksum;
    }
    public static int Calculate(
        byte[] bytes)
    {
        return bytes.Select(b => (int)b).Sum() % 256;
    }
}