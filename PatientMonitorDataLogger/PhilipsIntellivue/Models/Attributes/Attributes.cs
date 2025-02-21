namespace PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

public static class Attributes
{
    public static class Numerics
    {
        // VMO Static Context Group
        public static AttributeValueAssertion Type(NomenclatureReference type) => new((ushort)OIDType.NOM_ATTR_ID_TYPE, type);
        public static AttributeValueAssertion Handle(ushort handle) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_ID_HANDLE, handle);
        public static AttributeValueAssertion MetricSpecification(MetricSpecification metricSpecification) => new((ushort)OIDType.NOM_ATTR_METRIC_SPECN, metricSpecification);

        // VMO Dynamic Context Group
        public static AttributeValueAssertion Label(TextId text) => new((ushort)OIDType.NOM_ATTR_ID_LABEL, text);
        public static AttributeValueAssertion LabelString(IntellivueString label) => new((ushort)OIDType.NOM_ATTR_ID_LABEL_STRING, label);
        public static AttributeValueAssertion Color(IntellivueColors color) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_COLOR, (ushort)color);
        public static AttributeValueAssertion DisplayResolution(DisplayResolution displayResolution) => new((ushort)OIDType.NOM_ATTR_DISP_RES, displayResolution);

        // Metric Observed Value Group
        public static AttributeValueAssertion NumericObservedValue(NumericObservedValue value) => new((ushort)OIDType.NOM_ATTR_NU_VAL_OBS, value);
        public static AttributeValueAssertion CompoundNumericObservedValue(List<NumericObservedValue> values) => new((ushort)OIDType.NOM_ATTR_NU_CMPD_VAL_OBS, values);
        public static AttributeValueAssertion AbsoluteTimeStamp(AbsoluteTime time) => new((ushort)OIDType.NOM_ATTR_TIME_STAMP_ABS, time);
        public static AttributeValueAssertion RelativeTimeStamp(RelativeTime time) => new((ushort)OIDType.NOM_ATTR_TIME_STAMP_REL, time);
        public static AttributeValueAssertion MetricModality(MetricModality modality) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_METRIC_MODALITY, (ushort)modality);
    }
    public static class Waves
    {
        // VMO Static Context Group
        public static AttributeValueAssertion Handle(ushort handle) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_ID_HANDLE, handle);
        public static AttributeValueAssertion Type(NomenclatureReference type) => new((ushort)OIDType.NOM_ATTR_ID_TYPE, type);
        public static AttributeValueAssertion MetricSpecification(MetricSpecification metricSpecification) => new((ushort)OIDType.NOM_ATTR_METRIC_SPECN, metricSpecification);
        public static AttributeValueAssertion SampleArraySpecification(SampleArraySpecifications sampleArraySpecifications) => new((ushort)OIDType.NOM_ATTR_SA_SPECN, sampleArraySpecifications);
        public static AttributeValueAssertion SampleArrayFixedValueSpecification(List<SampleArrayFixedValueSpecificationEntry> entries) => new((ushort)OIDType.NOM_ATTR_SA_FIXED_VAL_SPECN, entries);
        public static AttributeValueAssertion SamplePeriod(RelativeTime time) => new((ushort)OIDType.NOM_ATTR_TIME_PD_SAMP, time);

        // VMO Dynamic Context Group
        public static AttributeValueAssertion Label(TextId text) => new((ushort)OIDType.NOM_ATTR_ID_LABEL, text);
        public static AttributeValueAssertion LabelString(IntellivueString label) => new((ushort)OIDType.NOM_ATTR_ID_LABEL_STRING, label);
        public static AttributeValueAssertion MetricState(MetricState state) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_METRIC_STAT, (ushort)state);
        public static AttributeValueAssertion UnitCode(OIDType code) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_UNIT_CODE, (ushort)code);
        public static AttributeValueAssertion Color(IntellivueColors color) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_COLOR, (ushort)color);
        public static AttributeValueAssertion MeasureMode(MeasureMode measureMode) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_MODE_MSMT, (ushort)measureMode);
        public static AttributeValueAssertion MetricInfoLabel(TextId text) => new((ushort)OIDType.NOM_ATTR_METRIC_INFO_LABEL, text);
        public static AttributeValueAssertion MetricInfoLabelString(IntellivueString label) => new((ushort)OIDType.NOM_ATTR_METRIC_INFO_LABEL_STR, label);
        public static AttributeValueAssertion ScaleAndRangeSpecification(ScaleAndRangeSpecification specs) => new((ushort)OIDType.NOM_ATTR_SCALE_SPECN_I16, specs);
        public static AttributeValueAssertion SampleArrayPhysiologicalRange(ScaledRange range) => new((ushort)OIDType.NOM_ATTR_SA_RANGE_PHYS_I16, range);
        public static AttributeValueAssertion VisualGrid(List<VisualGridEntry> grid) => new((ushort)OIDType.NOM_ATTR_GRID_VIS_I16, grid);
        public static AttributeValueAssertion SampleArrayCalibrationSpecification(CalibrationSpecification calibrationSpecification) => new((ushort)OIDType.NOM_ATTR_SA_CALIB_I16, calibrationSpecification);

        // Metric Observed Value Group
        public static AttributeValueAssertion SampleArrayObservedValue(SampleArrayObservedValue observation) => new((ushort)OIDType.NOM_ATTR_SA_VAL_OBS, observation);
        public static AttributeValueAssertion CompoundSampleArrayObservedValue(List<SampleArrayObservedValue> observations) => new((ushort)OIDType.NOM_ATTR_SA_CMPD_VAL_OBS, observations);
    }
    public static class Enumeration
    {
        // VMO Static Context Group
        public static AttributeValueAssertion Handle(ushort handle) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_ID_HANDLE, handle);
        public static AttributeValueAssertion Type(NomenclatureReference type) => new((ushort)OIDType.NOM_ATTR_ID_TYPE, type);
        public static AttributeValueAssertion MetricSpecification(MetricSpecification metricSpecification) => new((ushort)OIDType.NOM_ATTR_METRIC_SPECN, metricSpecification);

        // VMO Dynamic Context Group
        public static AttributeValueAssertion Label(TextId text) => new((ushort)OIDType.NOM_ATTR_ID_LABEL, text);
        public static AttributeValueAssertion LabelString(IntellivueString label) => new((ushort)OIDType.NOM_ATTR_ID_LABEL_STRING, label);
        public static AttributeValueAssertion Color(IntellivueColors color) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_COLOR, (ushort)color);

        // Metric Observed Value Group
        public static AttributeValueAssertion EnumObservedValue(EnumObservationValue observation) => new((ushort)OIDType.NOM_ATTR_VAL_ENUM_OBS, observation);
    }
    public static class System
    {
        // System Identification Attribute Group
        public static AttributeValueAssertion Handle(ushort handle) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_ID_HANDLE, handle);

        // System Application Attribute Group

        // System Production Attribute Group

    }
    public static class Alerts
    {
        // VMO Static Context Group
        public static AttributeValueAssertion Handle(ushort handle) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_ID_HANDLE, handle);
        public static AttributeValueAssertion Type(NomenclatureReference type) => new((ushort)OIDType.NOM_ATTR_ID_TYPE, type);

        // Alert Monitor Group
        public static AttributeValueAssertion DeviceAlertCondition(DeviceAlertCondition deviceAlertCondition) => new((ushort)OIDType.NOM_ATTR_DEV_AL_COND, deviceAlertCondition);
        public static AttributeValueAssertion DeviceTechnicalAlarmList(List<DeviceAlarmEntry> alarms) => new((ushort)OIDType.NOM_ATTR_AL_MON_T_AL_LIST, alarms);
        public static AttributeValueAssertion DevicePhysiologicalAlarmList(List<DeviceAlarmEntry> alarms) => new((ushort)OIDType.NOM_ATTR_AL_MON_P_AL_LIST, alarms);
    }
    public static class PatientDemographics
    {
        // Patient Demographics Attribute Group
        public static AttributeValueAssertion Handle(ushort handle) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_ID_HANDLE, handle);
        public static AttributeValueAssertion PatientState(PatientState patientState) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_PT_DEMOG_ST, (ushort)patientState);
        public static AttributeValueAssertion PatientType(PatientType patientType) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_PT_TYPE, (ushort)patientType);
        public static AttributeValueAssertion PatientPacedMode(PatientPacedMode pacedMode) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_PT_PACED_MODE, (ushort)pacedMode);
        public static AttributeValueAssertion GivenName(IntellivueString givenName) => new((ushort)OIDType.NOM_ATTR_PT_NAME_GIVEN, givenName);
        //public static AttributeValueAssertion MiddleName(IntellivueString givenName) => new((ushort)OIDType.NOM_ATTR_PT_NAME_MIDDLE, givenName);
        public static AttributeValueAssertion FamilyName(IntellivueString familyName) => new((ushort)OIDType.NOM_ATTR_PT_NAME_FAMILY, familyName);
        public static AttributeValueAssertion PatientId(IntellivueString patientId) => new((ushort)OIDType.NOM_ATTR_PT_LIFETIME_ID, patientId);
        //public static AttributeValueAssertion EncounterId(IntellivueString encounterId) => new((ushort)OIDType.NOM_ATTR_PT_ENCOUNTER_ID, encounterId);
        public static AttributeValueAssertion PatientSex(PatientSex sex) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_PT_SEX, (ushort)sex);
        public static AttributeValueAssertion DateOfBirth(AbsoluteTime dateOfBirth) => new((ushort)OIDType.NOM_ATTR_PT_DOB, dateOfBirth);
        public static AttributeValueAssertion PatientHeight(PatientMeasure height) => new((ushort)OIDType.NOM_ATTR_PT_HEIGHT, height);
        public static AttributeValueAssertion PatientWeight(PatientMeasure weight) => new((ushort)OIDType.NOM_ATTR_PT_WEIGHT, weight);
        public static AttributeValueAssertion PatientAge(PatientMeasure age) => new((ushort)OIDType.NOM_ATTR_PT_AGE, age);
        public static AttributeValueAssertion PatientBodySurfaceArea(PatientMeasure age) => new((ushort)OIDType.NOM_ATTR_PT_BSA, age);
        public static AttributeValueAssertion PatientBodySurfaceAreaFormula(PatientBodySurfaceAreaFormula formula) => BuildUshortAttribute((ushort)OIDType.NOM_ATTR_PT_BSA_FORMULA, (ushort)formula);
        public static AttributeValueAssertion Notes1(IntellivueString note) => new((ushort)OIDType.NOM_ATTR_PT_NOTES1, note);
        public static AttributeValueAssertion Notes2(IntellivueString note) => new((ushort)OIDType.NOM_ATTR_PT_NOTES2, note);
    }

    private static AttributeValueAssertion BuildUshortAttribute(
        ushort attributeId,
        ushort value)
    {
        return new(attributeId, new UshortAttributeValue(value));
    }
}