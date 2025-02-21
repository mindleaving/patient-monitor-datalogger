﻿using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class SingleContextPoll : ISerializable
{
    public SingleContextPoll(
        ushort contextId,
        List<ObservationPoll> observations)
    {
        ContextId = contextId;
        Observations = observations;
    }

    public ushort ContextId { get; }
    public List<ObservationPoll> Observations { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(ContextId),
            ..Observations.Serialize()
        ];
    }

    public static SingleContextPoll Read(
        BigEndianBinaryReader binaryReader)
    {
        var contextId = binaryReader.ReadUInt16();
        var pollInfo = List<ObservationPoll>.Read(binaryReader, ObservationPoll.Read);
        return new(contextId, pollInfo);
    }
}