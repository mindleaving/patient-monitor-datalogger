﻿using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class RemoteOperationResult : IRemoteOperation
{
    public RemoteOperationResult(
        ushort invokeId,
        DataExportCommandType commandType,
        ushort length,
        IRemoteOperationResultData data)
    {
        InvokeId = invokeId;
        CommandType = commandType;
        Length = length;
        Data = data;
    }

    public ushort InvokeId { get; }
    public DataExportCommandType CommandType { get; }
    public ushort Length { get; }
    public IRemoteOperationResultData Data { get; }

    public virtual byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(InvokeId),
            ..BigEndianBitConverter.GetBytes((ushort)CommandType),
            ..BigEndianBitConverter.GetBytes(Length),
            ..Data.Serialize()
        ];
    }

    public static RemoteOperationResult Read(
        BigEndianBinaryReader binaryReader)
    {
        var invokeId = binaryReader.ReadUInt16();
        var commandType = (DataExportCommandType)binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var data = ReadResultData(binaryReader, commandType);
        return new(
            invokeId,
            commandType,
            length,
            data);
    }

    public static IRemoteOperationResultData ReadResultData(
        BigEndianBinaryReader binaryReader,
        DataExportCommandType commandType)
    {
        return commandType switch
        {
            DataExportCommandType.EventReport => EventReportResultCommand.Read(binaryReader),
            DataExportCommandType.ConfirmedEventReport => EventReportResultCommand.Read(binaryReader),
            DataExportCommandType.Get => GetResultCommand.Read(binaryReader),
            DataExportCommandType.Set => SetResultCommand.Read(binaryReader),
            DataExportCommandType.ConfirmedSet => SetResultCommand.Read(binaryReader),
            DataExportCommandType.ConfirmedAction => ActionResultCommand.Read(binaryReader),
            _ => throw new ArgumentOutOfRangeException(nameof(commandType))
        };
    }
}