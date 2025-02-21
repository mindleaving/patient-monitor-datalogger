using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class InvalidScopeError : IRemoteOperationErrorData
{
    public InvalidScopeError(
        uint scope)
    {
        Scope = scope;
    }

    public uint Scope { get; }

    public static InvalidScopeError Read(
        BigEndianBinaryReader binaryReader)
    {
        var scope = binaryReader.ReadUInt32();
        return new(scope);
    }

    public byte[] Serialize()
    {
        return BigEndianBitConverter.GetBytes(Scope);
    }
}