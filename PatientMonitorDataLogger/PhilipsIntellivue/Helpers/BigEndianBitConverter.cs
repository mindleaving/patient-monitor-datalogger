namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public static class BigEndianBitConverter
{
    public static byte[] GetBytes(ushort value) => ToBigEndian(System.BitConverter.GetBytes(value));
    public static byte[] GetBytes(uint value) => ToBigEndian(System.BitConverter.GetBytes(value));
    public static byte[] GetBytes(int value) => ToBigEndian(System.BitConverter.GetBytes(value));

    private static byte[] ToBigEndian(byte[] bytes)
    {
        if(System.BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return bytes;
    }

    public static ushort ToUInt16(
        byte[] buffer,
        int offset = 0)
    {
        var bytes = ToBigEndian(buffer.Skip(offset).Take(sizeof(ushort)).ToArray());
        return System.BitConverter.ToUInt16(bytes);
    }

    public static uint ToUInt32(
        byte[] buffer,
        int offset = 0)
    {
        var bytes = ToBigEndian(buffer.Skip(offset).Take(sizeof(uint)).ToArray());
        return System.BitConverter.ToUInt32(bytes);
    }
}