using PatientMonitorDataLogger.API.Models.DataExport;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;

namespace PatientMonitorDataLogger.API.Workflow;

public class PhilipsIntellivueNumericsAndWavesExtractor
{
    private RelativeTimeTranslation? relativeTimeTranslation;

    public IEnumerable<IMonitorData> Extract(
        Guid logSessionId,
        ICommandMessage message)
    {
        if(message is not DataExportCommandMessage dataExportMessage)
            yield break;
        if(dataExportMessage.RemoteOperationData is not RemoteOperationResult remoteOperationResult)
            yield break;
        if(remoteOperationResult.CommandType != DataExportCommandType.ConfirmedAction)
            yield break;
        if(remoteOperationResult.Data is not ActionResultCommand actionResult)
            yield break;
        if(actionResult.Data is not PollMdiDataReply pollResult)
            yield break;
        var timestamp = GetAbsoluteTime(pollResult.RelativeTimeStamp);
        var numericsData = new NumericsData(logSessionId, timestamp, new Dictionary<MeasurementType, NumericsValue>());
        foreach (var singleContextPoll in pollResult.PollContexts.Values)
        {
            var contextId = singleContextPoll.ContextId;
            foreach (var observation in singleContextPoll.Observations.Values)
            {
                var handle = observation.Handle;
                foreach (var observationAttribute in observation.Attributes.Values)
                {
                    switch (observationAttribute.AttributeValue)
                    {
                        case SampleArrayObservedValue sampleArray:
                        {
                            if (!TryTranslateToMeasurementType(pollResult.ObjectType, sampleArray.PhysioId, out var measurementType))
                                continue;
                            if(sampleArray.State != MeasurementState.VALID)
                                continue;
                            var sampleRate = GetSampleRate(measurementType);
                            var translatedWaveSampleValues = TranslateWaveValues(sampleArray.Values.Values);
                            var waveData = new WaveData(
                                logSessionId,
                                measurementType,
                                timestamp,
                                sampleRate,
                                translatedWaveSampleValues);
                            yield return waveData;
                            break;
                        }
                        case NumericObservedValue numericsObservation:
                        {
                            if(!TryTranslateToMeasurementType(pollResult.ObjectType, numericsObservation.PhysioId, out var measurementType))
                                continue;
                            if(numericsObservation.State != MeasurementState.VALID)
                                continue;
                            numericsData.Values[measurementType] = new(
                                timestamp,
                                numericsObservation.Value.Value,
                                UnitCodesMap.GetValueOrDefault(numericsObservation.UnitCode, null));
                            break;
                        }
                    }
                }
            }
        }

        if (numericsData.Values.Count > 0)
            yield return numericsData;
    }

    private double GetSampleRate(
        MeasurementType measurementType)
    {
        return 128;
        switch (measurementType)
        {
            
        }

        throw new NotImplementedException();
    }

    private IList<float> TranslateWaveValues(
        ushort[] sampleValues)
    {
        return sampleValues.Select(x => (float)x).ToList();
    }

    private bool TryTranslateToMeasurementType(
        NomenclatureReference objectType,
        SCADAType physioId,
        out MeasurementType measurementType)
    {
        measurementType = MeasurementType.Undefined;
        if (objectType.Partition != NomenclaturePartition.NOM_PART_OBJ)
            return false;
        switch ((ObjectClass)objectType.Code)
        {
            case ObjectClass.NOM_MOC_VMO_METRIC_NU:
                measurementType = physioId switch
                {
                    SCADAType.NOM_AWAY_RESP_RATE => MeasurementType.RespirationRate,
                    SCADAType.NOM_PLETH_PULS_RATE => MeasurementType.HeartRateSpO2,
                    SCADAType.NOM_PULS_OXIM_SAT_O2 => MeasurementType.SpO2,
                    SCADAType.NOM_ECG_CARD_BEAT_RATE => MeasurementType.HeartRateEcg,
                    // TODO: Add all relevant
                    _ => MeasurementType.Undefined
                };
                return measurementType != MeasurementType.Undefined;
            case ObjectClass.NOM_MOC_VMO_METRIC_SA_RT:
                measurementType = physioId switch
                {
                    SCADAType.NOM_ECG_ELEC_POTL_I => MeasurementType.EcgLeadI,
                    SCADAType.NOM_PLETH => MeasurementType.Pleth,
                    // TODO: Add all relevant
                    _ => MeasurementType.Undefined
                };
                return measurementType != MeasurementType.Undefined;
        }
        return false;
    }

    private DateTime GetAbsoluteTime(
        RelativeTime relativeTime)
    {
        relativeTimeTranslation ??= new RelativeTimeTranslation(DateTime.UtcNow, relativeTime.Ticks);
        return relativeTimeTranslation.GetAbsoluteTime(relativeTime);
    }
}