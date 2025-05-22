using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Converters;

public class AttributeValueAssertionJsonConverter : JsonConverter<AttributeValueAssertion>
{
    public override bool CanWrite => false;

    public override void WriteJson(
        JsonWriter writer,
        AttributeValueAssertion? value,
        JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }

    public override AttributeValueAssertion? ReadJson(
        JsonReader reader,
        Type objectType,
        AttributeValueAssertion? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var attributeId = jObject.Value<ushort>(nameof(AttributeValueAssertion.AttributeId));
        var attributeValueToken = jObject.GetValue(nameof(AttributeValueAssertion.AttributeValue));
        var attributeValue = ReadAttributeValue(attributeId, attributeValueToken, serializer);
        if (attributeValue == null)
            return null;
        return new AttributeValueAssertion(attributeId, attributeValue);
    }

    private ISerializable? ReadAttributeValue(
        ushort attributeId,
        JToken? jToken,
        JsonSerializer serializer)
    {
        if (jToken == null)
            return null;
        ISerializable model = attributeId switch
        {
            (ushort)OIDType.NOM_ATTR_ID_TYPE => new NomenclatureReference(),
            (ushort)OIDType.NOM_ATTR_ID_HANDLE => new Handle(),
            (ushort)OIDType.NOM_ATTR_METRIC_SPECN => new MetricSpecification(),
            (ushort)OIDType.NOM_ATTR_ID_LABEL => new TextId(),
            (ushort)OIDType.NOM_ATTR_ID_LABEL_STRING => new IntellivueString(),
            (ushort)OIDType.NOM_ATTR_COLOR => new EnumAttributeValue<IntellivueColors>(),
            (ushort)OIDType.NOM_ATTR_DISP_RES => new DisplayResolution(),
            (ushort)OIDType.NOM_ATTR_NU_VAL_OBS => new NumericObservedValue(),
            (ushort)OIDType.NOM_ATTR_NU_CMPD_VAL_OBS => new Models.List<NumericObservedValue>(),
            (ushort)OIDType.NOM_ATTR_TIME_STAMP_ABS => new AbsoluteTime(),
            (ushort)OIDType.NOM_ATTR_TIME_STAMP_REL => new RelativeTime(),
            (ushort)OIDType.NOM_ATTR_METRIC_MODALITY => new EnumAttributeValue<MetricModality>(),
            (ushort)OIDType.NOM_ATTR_SA_SPECN => new SampleArraySpecifications(),
            (ushort)OIDType.NOM_ATTR_SA_FIXED_VAL_SPECN => new Models.List<SampleArrayFixedValueSpecificationEntry>(),
            (ushort)OIDType.NOM_ATTR_TIME_PD_SAMP => new RelativeTime(),
            (ushort)OIDType.NOM_ATTR_METRIC_STAT => new EnumAttributeValue<MetricState>(),
            (ushort)OIDType.NOM_ATTR_UNIT_CODE => new EnumAttributeValue<OIDType>(),
            (ushort)OIDType.NOM_ATTR_MODE_MSMT => new EnumAttributeValue<MeasureMode>(),
            (ushort)OIDType.NOM_ATTR_METRIC_INFO_LABEL => new TextId(),
            (ushort)OIDType.NOM_ATTR_SCALE_SPECN_I16 => new ScaleAndRangeSpecification(),
            (ushort)OIDType.NOM_ATTR_SA_RANGE_PHYS_I16 => new ScaledRange(),
            (ushort)OIDType.NOM_ATTR_GRID_VIS_I16 => new Models.List<VisualGridEntry>(),
            (ushort)OIDType.NOM_ATTR_SA_CALIB_I16 => new CalibrationSpecification(),
            (ushort)OIDType.NOM_ATTR_SA_VAL_OBS => new SampleArrayObservedValue(),
            (ushort)OIDType.NOM_ATTR_SA_CMPD_VAL_OBS => new Models.List<SampleArrayObservedValue>(),
            (ushort)OIDType.NOM_ATTR_VAL_ENUM_OBS => new EnumObservationValue(),
            (ushort)OIDType.NOM_ATTR_DEV_AL_COND => new DeviceAlertCondition(),
            (ushort)OIDType.NOM_ATTR_AL_MON_T_AL_LIST => new Models.List<DeviceAlarmEntry>(),
            (ushort)OIDType.NOM_ATTR_PT_DEMOG_ST => new EnumAttributeValue<PatientState>(),
            (ushort)OIDType.NOM_ATTR_PT_TYPE => new EnumAttributeValue<PatientType>(),
            (ushort)OIDType.NOM_ATTR_PT_PACED_MODE => new EnumAttributeValue<PatientPacedMode>(),
            (ushort)OIDType.NOM_ATTR_PT_NAME_GIVEN => new IntellivueString(),
            //(ushort)OIDType.NOM_ATTR_PT_NAME_MIDDLE => new IntellivueString(),
            (ushort)OIDType.NOM_ATTR_PT_NAME_FAMILY => new IntellivueString(),
            (ushort)OIDType.NOM_ATTR_PT_LIFETIME_ID => new IntellivueString(),
            //(ushort)OIDType.NOM_ATTR_PT_ENCOUNTER_ID => new IntellivueString(),
            (ushort)OIDType.NOM_ATTR_PT_SEX => new EnumAttributeValue<PatientSex>(),
            (ushort)OIDType.NOM_ATTR_PT_DOB => new AbsoluteTime(),
            (ushort)OIDType.NOM_ATTR_PT_HEIGHT => new PatientMeasure(),
            (ushort)OIDType.NOM_ATTR_PT_WEIGHT => new PatientMeasure(),
            (ushort)OIDType.NOM_ATTR_PT_AGE => new PatientMeasure(),
            (ushort)OIDType.NOM_ATTR_PT_BSA => new PatientMeasure(),
            (ushort)OIDType.NOM_ATTR_PT_BSA_FORMULA => new EnumAttributeValue<PatientBodySurfaceAreaFormula>(),
            (ushort)OIDType.NOM_ATTR_PT_NOTES1 => new IntellivueString(),
            (ushort)OIDType.NOM_ATTR_PT_NOTES2 => new IntellivueString(),
            (ushort)OIDType.NOM_ATTR_TIME_PD_POLL => new RelativeTime(),
            (ushort)OIDType.NOM_ATTR_POLL_OBJ_PRIO_NUM => new UshortAttributeValue(),
            (ushort)OIDType.NOM_ATTR_POLL_NU_PRIO_LIST => new Models.List<TextId>(),
            (ushort)OIDType.NOM_ATTR_POLL_RTSA_PRIO_LIST => new Models.List<TextId>(),
            _ => new UnknownAttributeValue()
        };
        serializer.Populate(jToken.CreateReader(), model);
        return model;
    }
}