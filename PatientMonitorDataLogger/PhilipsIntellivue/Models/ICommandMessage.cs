using Newtonsoft.Json;
using PatientMonitorDataLogger.PhilipsIntellivue.Converters;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

[JsonConverter(typeof(CommandMessageJsonConverter))]
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