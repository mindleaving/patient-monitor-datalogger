using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class AttributeValueAssertion : ISerializable
{
    public AttributeValueAssertion(
        ushort attributeId,
        ISerializable attributeValue)
    {
        var attributeValueBytes = attributeValue.Serialize();
        AttributeId = attributeId;
        Length = (ushort)attributeValueBytes.Length;
        AttributeValue = attributeValue;
    }

    public ushort AttributeId { get; }
    public ushort Length { get; }
    public ISerializable AttributeValue { get; }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes(AttributeId),
            ..BigEndianBitConverter.GetBytes(Length),
            ..AttributeValue.Serialize()
        ];
    }

    public static AttributeValueAssertion Read(
        BigEndianBinaryReader binaryReader,
        AttributeContext context)
    {
        var attributeId = binaryReader.ReadUInt16();
        var length = binaryReader.ReadUInt16();
        var attributeValueBytes = new byte[length];
        var bytesRead = binaryReader.Read(attributeValueBytes);
        if (bytesRead != length)
            throw new EndOfStreamException();
        if(!TryParseTypedAttributeValue(attributeId, attributeValueBytes, context, out var attributeValue))
            attributeValue = new UnknownAttributeValue(attributeValueBytes);
        return new AttributeValueAssertion(attributeId, attributeValue);
    }

    private static bool TryParseTypedAttributeValue(
        ushort attributeId,
        byte[] attributeValueBytes,
        AttributeContext context,
        out ISerializable attributeValue)
    {
        var binaryReader = new BigEndianBinaryReader(new MemoryStream(attributeValueBytes));
        try
        {
            if (context.MessageType == CommandMessageType.Association)
            {
                switch (attributeId)
                {
                    case (ushort)ProtocolIdentification.NOM_POLL_PROFILE_SUPPORT:
                        attributeValue = PollProfileSupport.Read(binaryReader, context);
                        return true;
                    case (ushort)ProtocolIdentification.NOM_ATTR_POLL_PROFILE_EXT:
                        attributeValue = ExtendedPollProfile.Read(binaryReader, context);
                        return true;
                }
            }
            attributeValue = attributeId switch
            {
                (ushort)OIDType.NOM_ATTR_ID_TYPE => NomenclatureReference.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_ID_HANDLE => Handle.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_METRIC_SPECN => MetricSpecification.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_ID_LABEL => TextId.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_ID_LABEL_STRING => IntellivueString.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_COLOR => EnumAttributeValue<IntellivueColors>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_DISP_RES => DisplayResolution.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_NU_VAL_OBS => NumericObservedValue.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_NU_CMPD_VAL_OBS => List<NumericObservedValue>.Read(binaryReader, NumericObservedValue.Read),
                (ushort)OIDType.NOM_ATTR_TIME_STAMP_ABS => AbsoluteTime.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_TIME_STAMP_REL => RelativeTime.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_METRIC_MODALITY => EnumAttributeValue<MetricModality>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_SA_SPECN => SampleArraySpecifications.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_SA_FIXED_VAL_SPECN => List<SampleArrayFixedValueSpecificationEntry>.Read(binaryReader, SampleArrayFixedValueSpecificationEntry.Read),
                (ushort)OIDType.NOM_ATTR_TIME_PD_SAMP => RelativeTime.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_METRIC_STAT => EnumAttributeValue<MetricState>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_UNIT_CODE => EnumAttributeValue<OIDType>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_MODE_MSMT => EnumAttributeValue<MeasureMode>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_METRIC_INFO_LABEL => TextId.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_SCALE_SPECN_I16 => ScaleAndRangeSpecification.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_SA_RANGE_PHYS_I16 => ScaledRange.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_GRID_VIS_I16 => List<VisualGridEntry>.Read(binaryReader, VisualGridEntry.Read),
                (ushort)OIDType.NOM_ATTR_SA_CALIB_I16 => CalibrationSpecification.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_SA_VAL_OBS => SampleArrayObservedValue.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_SA_CMPD_VAL_OBS => List<SampleArrayObservedValue>.Read(binaryReader, SampleArrayObservedValue.Read),
                (ushort)OIDType.NOM_ATTR_VAL_ENUM_OBS => EnumObservationValue.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_DEV_AL_COND => DeviceAlertCondition.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_AL_MON_T_AL_LIST => List<DeviceAlarmEntry>.Read(binaryReader, DeviceAlarmEntry.Read),
                (ushort)OIDType.NOM_ATTR_PT_DEMOG_ST => EnumAttributeValue<PatientState>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_PT_TYPE => EnumAttributeValue<PatientType>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_PT_PACED_MODE => EnumAttributeValue<PatientPacedMode>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_PT_NAME_GIVEN => IntellivueString.Read(binaryReader),
                //(ushort)OIDType.NOM_ATTR_PT_NAME_MIDDLE => IntellivueString.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_PT_NAME_FAMILY => IntellivueString.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_PT_LIFETIME_ID => IntellivueString.Read(binaryReader),
                //(ushort)OIDType.NOM_ATTR_PT_ENCOUNTER_ID => IntellivueString.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_PT_SEX => EnumAttributeValue<PatientSex>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_PT_DOB => AbsoluteTime.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_PT_HEIGHT => PatientMeasure.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_PT_WEIGHT => PatientMeasure.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_PT_AGE => PatientMeasure.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_PT_BSA => PatientMeasure.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_PT_BSA_FORMULA => EnumAttributeValue<PatientBodySurfaceAreaFormula>.Parse(attributeValueBytes),
                (ushort)OIDType.NOM_ATTR_PT_NOTES1 => IntellivueString.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_PT_NOTES2 => IntellivueString.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_TIME_PD_POLL => RelativeTime.Read(binaryReader),
                (ushort)OIDType.NOM_ATTR_POLL_OBJ_PRIO_NUM => new UshortAttributeValue(binaryReader.ReadUInt16()),
                (ushort)OIDType.NOM_ATTR_POLL_NU_PRIO_LIST => List<TextId>.Read(binaryReader, TextId.Read),
                (ushort)OIDType.NOM_ATTR_POLL_RTSA_PRIO_LIST => List<TextId>.Read(binaryReader, TextId.Read),
                _ => new UnknownAttributeValue(attributeValueBytes)
            };
            return true;
        }
        catch
        {
            attributeValue = null;
            return false;
        }
    }
}