﻿using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public class DeviceAlarmEntry : ISerializable
{
    public DeviceAlarmEntry(
        OIDType source,
        OIDType code,
        AlertType type,
        AlertState state,
        ManagedObjectId obj,
        AlertInfoType additionalInfoType,
        ushort length,
        AlarmMonitorGeneralInfo additionalInfo)
    {
        Source = source;
        Code = code;
        Type = type;
        State = state;
        Object = obj;
        AdditionalInfoType = additionalInfoType;
        Length = length;
        AdditionalInfo = additionalInfo;
    }

    public OIDType Source { get; }
    public OIDType Code { get; }
    public AlertType Type { get; }
    public AlertState State { get; }
    public ManagedObjectId Object { get; }
    public AlertInfoType AdditionalInfoType { get; }
    public ushort Length { get; }
    public AlarmMonitorGeneralInfo AdditionalInfo { get; }

    public static DeviceAlarmEntry Read(
        BigEndianBinaryReader binaryReader)
    {
        var source = (OIDType)binaryReader.ReadUInt16();
        var code = (OIDType)binaryReader.ReadUInt16();
        var type = (AlertType)binaryReader.ReadUInt16();
        var state = (AlertState)binaryReader.ReadUInt16();
        var obj = ManagedObjectId.Read(binaryReader);
        var additionalInfoType = (AlertInfoType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var additionalInfo = additionalInfoType switch
        {
            AlertInfoType.GEN_ALMON_INFO => AlarmMonitorGeneralInfo.Read(binaryReader),
            AlertInfoType.STR_ALMON_INFO => AlarmMonitorStringInfo.Read(binaryReader),
            _ => throw new ArgumentOutOfRangeException(nameof(additionalInfoType))
        };
        return new(
            source,
            code,
            type,
            state,
            obj,
            additionalInfoType,
            length,
            additionalInfo);
    }

    public byte[] Serialize()
    {
        var additionalInfoBytes = AdditionalInfo.Serialize();
        return
        [
            ..BigEndianBitConverter.GetBytes((ushort)Source),
            ..BigEndianBitConverter.GetBytes((ushort)Code),
            ..BigEndianBitConverter.GetBytes((ushort)Type),
            ..BigEndianBitConverter.GetBytes((ushort)State),
            ..Object.Serialize(),
            ..BigEndianBitConverter.GetBytes((ushort)AdditionalInfoType),
            ..BigEndianBitConverter.GetBytes(additionalInfoBytes.Length),
            ..additionalInfoBytes
        ];

    }
}