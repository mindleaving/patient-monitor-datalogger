namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public interface ICommandMessage : ISerializable
{
    /// <summary>
    /// Not part of protocol. Do not serialize!
    /// </summary>
    CommandMessageType MessageType { get; }
}

public enum CommandMessageType
{
    Association = 1,
    DataExport = 2
}