﻿using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ObservationPoll : ISerializable
{
    public ObservationPoll(
        ushort handle,
        List<AttributeValueAssertion> attributes)
    {
        Handle = handle;
        Attributes = attributes;
    }

    public ushort Handle { get; }
    public List<AttributeValueAssertion> Attributes { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(Handle),
            ..Attributes.Serialize()
        ];
    }

    public static ObservationPoll Read(
        BigEndianBinaryReader binaryReader)
    {
        var handle = binaryReader.ReadUInt16();
        var attributes = List<AttributeValueAssertion>.Read(binaryReader, AttributeValueAssertion.Read);
        return new(handle, attributes);
    }
}