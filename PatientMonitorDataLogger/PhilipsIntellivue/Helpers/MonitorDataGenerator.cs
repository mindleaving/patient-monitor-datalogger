using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class MonitorDataGenerator
{
    private readonly Dictionary<SCADAType, NumericsGenerator> numericsGenerators = new()
    {
        { SCADAType.NOM_PLETH_PULS_RATE, new NumericsGenerator(48, 53) },
        { SCADAType.NOM_ECG_CARD_BEAT_RATE, new NumericsGenerator(48, 53) },
        { SCADAType.NOM_PULS_OXIM_SAT_O2, new NumericsGenerator(96, 100) },
        { SCADAType.NOM_AWAY_RESP_RATE, new NumericsGenerator(10, 15) },
    };
    private readonly Dictionary<SCADAType, WaveGenerator> waveGenerators = new()
    {
        { SCADAType.NOM_ECG_ELEC_POTL_II, new WaveGenerator(SampleData.EcgII) },
        { SCADAType.NOM_PLETH, new WaveGenerator(SampleData.Pleth) },
        { SCADAType.NOM_RESP, new WaveGenerator(SampleData.Respiration) },
    };

    public Models.List<ObservationPoll> Generate(
        NomenclatureReference objectType,
        OIDType attributeGroup)
    {
        if (objectType == PollObjectTypes.Alerts)
        {
            if (attributeGroup == PollAttributeGroups.Alerts.AlertMonitor)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Alerts.VmoStaticContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        if (objectType == PollObjectTypes.MDS)
        {
            if (attributeGroup == PollAttributeGroups.System.SystemIdentification)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.System.SystemApplication)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.System.SystemProduction)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        if(objectType == PollObjectTypes.Numerics)
        {
            if (attributeGroup == PollAttributeGroups.Numerics.VmoStaticContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Numerics.VmoDynamicContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Numerics.MetricObservedValue)
            {
                return new(
                [
                    new ObservationPoll(0, new([
                        GenerateNumericsObservation(SCADAType.NOM_PLETH_PULS_RATE),
                        GenerateNumericsObservation(SCADAType.NOM_ECG_CARD_BEAT_RATE),
                        GenerateNumericsObservation(SCADAType.NOM_PULS_OXIM_SAT_O2),
                        GenerateNumericsObservation(SCADAType.NOM_AWAY_RESP_RATE),
                    ]))
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        if(objectType == PollObjectTypes.PatientDemographics)
        {
            if (attributeGroup == PollAttributeGroups.PatientDemographics.All)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                    new ObservationPoll(0, new([
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_ID_HANDLE, new Handle(0)),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_DEMOG_ST, new EnumAttributeValue<PatientState>(PatientState.ADMITTED, BigEndianBitConverter.GetBytes((ushort)PatientState.ADMITTED))),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_TYPE, new EnumAttributeValue<PatientType>(PatientType.ADULT, BigEndianBitConverter.GetBytes((ushort)PatientType.ADULT))),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_PACED_MODE, new EnumAttributeValue<PatientPacedMode>(PatientPacedMode.NotPaced, BigEndianBitConverter.GetBytes((ushort)PatientPacedMode.NotPaced))),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_NAME_GIVEN, new IntellivueString("Jan")),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_NAME_FAMILY, new IntellivueString("Scholtyssek")),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_LIFETIME_ID, new IntellivueString("19891117-XMWT3")),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_SEX, new EnumAttributeValue<PatientSex>(PatientSex.MALE, BigEndianBitConverter.GetBytes((ushort)PatientSex.MALE))),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_DOB, new AbsoluteTime(19, 89, 11, 17, 0, 0, 0)),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_HEIGHT, new PatientMeasure(new(182), UnitCodes.NOM_DIM_CENTI_M)),
                        new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_PT_WEIGHT, new PatientMeasure(new(60), UnitCodes.NOM_DIM_KILO_G)),
                    ]))
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        if(objectType == PollObjectTypes.Waves)
        {
            if (attributeGroup == PollAttributeGroups.Numerics.VmoStaticContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Numerics.VmoDynamicContext)
            {
                return new(
                [
                    // TODO: Add at least all mandatory attributes
                ]);
            }

            if (attributeGroup == PollAttributeGroups.Numerics.MetricObservedValue)
            {
                return new(
                [
                    new(2107, new([
                        GenerateSampleArray(SCADAType.NOM_ECG_ELEC_POTL_II, 128),
                        GenerateSampleArray(SCADAType.NOM_PLETH, 32),
                        GenerateSampleArray(SCADAType.NOM_RESP, 16)
                    ]))
                ]);
            }

            throw new ArgumentOutOfRangeException(nameof(attributeGroup), $"Unknown attribute group {attributeGroup} for {objectType}");
        }

        throw new ArgumentOutOfRangeException(nameof(objectType), $"Unknown poll object type {objectType}");
    }

    private AttributeValueAssertion GenerateNumericsObservation(
        SCADAType physioId)
    {
        NumericObservedValue observation;
        if (numericsGenerators.TryGetValue(physioId, out var numericsGenerator))
        {
            var value = numericsGenerator.GetValue();
            observation = new NumericObservedValue(physioId, MeasurementState.VALID, UnitCodes.NOM_DIM_PER_MIN, new(value));
        }
        else
        {
            observation = new NumericObservedValue(physioId, MeasurementState.INVALID, UnitCodes.NOM_DIM_PER_MIN, new(float.NaN));
        }

        return new AttributeValueAssertion(
            (ushort)OIDType.NOM_ATTR_NU_VAL_OBS, 
            observation);
    }

    private AttributeValueAssertion GenerateSampleArray(
        SCADAType physioId,
        int sampleCount)
    {
        SampleArrayObservedValue sampleArray;
        if (waveGenerators.TryGetValue(physioId, out var waveGenerator))
        {
            var waveData = waveGenerator.GetMany(sampleCount).ToArray();
            sampleArray = new SampleArrayObservedValue(physioId, MeasurementState.VALID, new(waveData));
        }
        else
        {
            sampleArray = new SampleArrayObservedValue(physioId, MeasurementState.INVALID, new(Enumerable.Repeat<ushort>(8192, sampleCount).ToArray()));
        }

        return new AttributeValueAssertion((ushort)OIDType.NOM_ATTR_SA_VAL_OBS, sampleArray);
    }
}