using System.Collections;
using System.Diagnostics.CodeAnalysis;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;
using PatientMonitorDataLogger.Shared.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.DataProcessing;

public class PhilipsIntellivueNumericsAndWavesExtractor
{
    private RelativeTimeTranslation? defaultRelativeTimeTranslation;

    public IEnumerable<IMonitorData> Extract(
        LinkedCommandMessageBundle messageBundle,
        IList<MeasurementState> excludedMeasurementStates,
        Func<uint,RelativeTimeTranslation>? relativeTimeTranslationFactory = null)
    {
        var numericsValues = new Dictionary<string, NumericsValue>();
        DateTime? timestamp = null;
        foreach (var pollResult in GetPollResults(messageBundle))
        {
            timestamp ??= GetAbsoluteTime(pollResult.RelativeTimeStamp, relativeTimeTranslationFactory);
            foreach (var observation in ExtractObservations(pollResult))
            {
                switch (observation)
                {
                    case SampleArrayObservedValue sampleArray:
                    {
                        if(!IsSampleArrayValid(sampleArray, pollResult.ObjectType, excludedMeasurementStates, out var measurementType))
                            continue;
                        var waveData = CreateWaveData(sampleArray, measurementType, timestamp.Value);
                        yield return waveData;
                        break;
                    }
                    case PhilipsIntellivue.Models.List<SampleArrayObservedValue> listOfSampleArrays:
                    {
                        foreach (var sampleArray in listOfSampleArrays.Values)
                        {
                            if(!IsSampleArrayValid(sampleArray, pollResult.ObjectType, excludedMeasurementStates, out var measurementType))
                                continue;
                            var waveData = CreateWaveData(sampleArray, measurementType, timestamp.Value);
                            yield return waveData;
                        }
                        break;
                    }
                    case NumericObservedValue numericsObservation:
                    {
                        if(!IsNumericsObservationValid(numericsObservation, pollResult.ObjectType, excludedMeasurementStates, out var measurementType))
                            continue;
                        numericsValues[measurementType] = CreateNumericsDataValue(timestamp.Value, numericsObservation);
                        break;
                    }
                    case PhilipsIntellivue.Models.List<NumericObservedValue> listOfNumericsObservations:
                    {
                        foreach (var numericsObservation in listOfNumericsObservations.Values)
                        {
                            if(!IsNumericsObservationValid(numericsObservation, pollResult.ObjectType, excludedMeasurementStates, out var measurementType))
                                continue;
                            numericsValues[measurementType] = CreateNumericsDataValue(timestamp.Value, numericsObservation);
                        }
                        break;
                    }
                    //case AbsoluteTime:
                    //case DeviceAlertCondition:
                    //case UnknownAttributeValue:
                    //case PhilipsIntellivue.Models.List<DeviceAlarmEntry>:
                    //    break;
                    //default:
                    //{
                    //    Console.WriteLine($"Unsupported data type {observationAttribute.AttributeValue.GetType()}");
                    //    break;
                    //}
                }
            }
        }

        if (timestamp.HasValue && numericsValues.Count > 0)
            yield return new NumericsData(timestamp.Value, numericsValues);
    }

    private static IEnumerable<ISerializable> ExtractObservations(
        PollMdiDataReply pollResult)
    {
        return from singleContextPoll in pollResult.PollContexts.Values 
            from observation in singleContextPoll.Observations.Values 
            from observationAttribute in observation.Attributes.Values 
            select observationAttribute.AttributeValue;
    }

    private IEnumerable<PollMdiDataReply> GetPollResults(
        LinkedCommandMessageBundle messageBundle)
    {
        foreach (var message in messageBundle.Messages)
        {
            if(message is not DataExportCommandMessage dataExportMessage)
                continue;
            if(dataExportMessage.RemoteOperationData is not RemoteOperationResult remoteOperationResult)
                continue;
            if(remoteOperationResult.CommandType != DataExportCommandType.ConfirmedAction)
                continue;
            if(remoteOperationResult.Data is not ActionResultCommand actionResult)
                continue;
            if(actionResult.Data is not PollMdiDataReply pollResult)
                continue;
            yield return pollResult;
        }
    }

    private bool IsSampleArrayValid(
        SampleArrayObservedValue sampleArray,
        NomenclatureReference objectType,
        IList<MeasurementState> excludedMeasurementStates,
        [NotNullWhen(true)] out string? measurementType)
    {
        measurementType = null;
        if (sampleArray.Values.Values.Length == 0)
            return false;
        if (!TryTranslateToMeasurementType(objectType, sampleArray.PhysioId, out measurementType))
            return false;
        if (excludedMeasurementStates.Contains(sampleArray.State))
            return false;
        return true;
    }

    private WaveData CreateWaveData(
        SampleArrayObservedValue sampleArray,
        string measurementType,
        DateTime timestamp)
    {
        var sampleRate = sampleArray.Values.Values.Length * 4; // 4 messages per second
        var translatedWaveSampleValues = TranslateWaveValues(sampleArray.Values.Values);
        var waveData = new WaveData(
            measurementType,
            timestamp,
            sampleRate,
            translatedWaveSampleValues);
        return waveData;
    }

    private bool IsNumericsObservationValid(
        NumericObservedValue numericsObservation,
        NomenclatureReference objectType,
        IList<MeasurementState> excludedMeasurementStates,
        [NotNullWhen(true)] out string? measurementType)
    {
        if (!TryTranslateToMeasurementType(objectType, numericsObservation.PhysioId, out measurementType))
            return false;
        if (excludedMeasurementStates.Contains(numericsObservation.State))
            return false;
        return true;
    }

    private static NumericsValue CreateNumericsDataValue(
        DateTime timestamp,
        NumericObservedValue numericsObservation)
    {
        return new(
            timestamp,
            numericsObservation.Value.Value,
            UnitCodesMap.GetValueOrDefault(numericsObservation.UnitCode, null),
            numericsObservation.State);
    }

    private IList<float> TranslateWaveValues(
        ushort[] sampleValues)
    {
        return sampleValues.Select(x => (float)x).ToList();
    }

    private bool TryTranslateToMeasurementType(
        NomenclatureReference objectType,
        SCADAType physioId,
        [NotNullWhen(true)] out string? measurementType)
    {
        measurementType = physioId.ToString();
        return true;
        //measurementType = null;
        //if (objectType.Partition != NomenclaturePartition.NOM_PART_OBJ)
        //    return false;
        //switch ((ObjectClass)objectType.Code)
        //{
        //    case ObjectClass.NOM_MOC_VMO_METRIC_NU:
        //        measurementType = physioId switch
        //        {
        //            SCADAType.NOM_AWAY_RESP_RATE => MeasurementType.RespirationRate,
        //            SCADAType.NOM_PLETH_PULS_RATE => MeasurementType.HeartRateSpO2,
        //            SCADAType.NOM_PULS_OXIM_SAT_O2 => MeasurementType.SpO2,
        //            SCADAType.NOM_ECG_CARD_BEAT_RATE => MeasurementType.HeartRateEcg,
        //            // TODO: Add all relevant
        //            _ => MeasurementType.Undefined
        //        };
        //        return measurementType != MeasurementType.Undefined;
        //    case ObjectClass.NOM_MOC_VMO_METRIC_SA_RT:
        //        measurementType = physioId switch
        //        {
        //            SCADAType.NOM_ECG_ELEC_POTL_I => MeasurementType.EcgLeadI,
        //            SCADAType.NOM_PLETH => MeasurementType.Pleth,
        //            // TODO: Add all relevant
        //            _ => MeasurementType.Undefined
        //        };
        //        return measurementType != MeasurementType.Undefined;
        //}
        //return false;
    }

    private DateTime GetAbsoluteTime(
        RelativeTime relativeTime,
        Func<uint, RelativeTimeTranslation>? relativeTimeTranslationFactory = null)
    {
        if(defaultRelativeTimeTranslation == null)
        {
            defaultRelativeTimeTranslation = relativeTimeTranslationFactory != null 
                ? relativeTimeTranslationFactory(relativeTime.Ticks)
                : RelativeTimeTranslation.PhilipsIntellivue(DateTime.UtcNow, relativeTime.Ticks);
        }
        return defaultRelativeTimeTranslation.GetAbsoluteTime(relativeTime);
    }
}