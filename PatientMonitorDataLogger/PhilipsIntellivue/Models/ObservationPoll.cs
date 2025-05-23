﻿using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class ObservationPoll : ISerializable
{
    public ObservationPoll() { }

    public ObservationPoll(
        ushort handle,
        List<AttributeValueAssertion> attributes)
    {
        Handle = handle;
        Attributes = attributes;
    }

    public ushort Handle { get; set; }
    public List<AttributeValueAssertion> Attributes { get; set; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(Handle),
            ..Attributes.Serialize()
        ];
    }

    public static ObservationPoll Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var handle = binaryReader.ReadUInt16();
        var attributes = List<AttributeValueAssertion>.Read(binaryReader, x => AttributeValueAssertion.Read(x, context));
        return new(handle, attributes);
    }
}