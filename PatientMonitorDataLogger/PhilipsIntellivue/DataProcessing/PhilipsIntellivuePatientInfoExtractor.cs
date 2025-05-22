using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;
using PatientMonitorDataLogger.Shared;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.DataProcessing;

public class PhilipsIntellivuePatientInfoExtractor
{
    public bool TryExtract(
        Guid logSessionId,
        LinkedCommandMessageBundle messageBundle,
        out PatientInfo? patientInfo)
    {
        patientInfo = null;
        foreach (var pollResult in GetPollResults(messageBundle))
        {
            foreach (var attribute in ExtractAttributes(pollResult))
            {
                switch ((OIDType)attribute.AttributeId)
                {
                    case OIDType.NOM_ATTR_PT_NAME_GIVEN:
                    {
                        var attributeValue = attribute.AttributeValue as IntellivueString;
                        if (attributeValue != null)
                        {
                            patientInfo ??= new PatientInfo(logSessionId);
                            patientInfo.FirstName = attributeValue.Value;
                        }
                        break;
                    }
                    case OIDType.NOM_ATTR_PT_NAME_FAMILY:
                    {
                        var attributeValue = attribute.AttributeValue as IntellivueString;
                        if (attributeValue != null)
                        {
                            patientInfo ??= new PatientInfo(logSessionId);
                            patientInfo.LastName = attributeValue.Value;
                        }
                        break;
                    }
                    case OIDType.NOM_ATTR_PT_ID:
                    {
                        var attributeValue = attribute.AttributeValue as IntellivueString;
                        if (attributeValue != null)
                        {
                            patientInfo ??= new PatientInfo(logSessionId);
                            patientInfo.PatientId = attributeValue.Value;
                        }
                        break;
                    }
                    //case OIDType.NOM_ATTR_PT_ENCOUNTER_ID:
                    //{
                    //    var attributeValue = observationAttribute.AttributeValue as IntellivueString;
                    //    if (attributeValue != null)
                    //    {
                    //        patientInfo ??= new PatientInfo();
                    //        patientInfo.EncounterId = attributeValue.Value;
                    //    }
                    //    break;
                    //}
                    case OIDType.NOM_ATTR_PT_SEX:
                    {
                        var attributeValue = attribute.AttributeValue as EnumAttributeValue<PatientSex>;
                        if (attributeValue != null)
                        {
                            patientInfo ??= new PatientInfo(logSessionId);
                            patientInfo.Sex = attributeValue.Value switch
                            {
                                PatientSex.SEX_UNKNOWN => Sex.Undefined,
                                PatientSex.MALE => Sex.Male,
                                PatientSex.FEMALE => Sex.Female,
                                PatientSex.SEX_UNSPECIFIED => Sex.Undefined,
                                _ => throw new ArgumentOutOfRangeException()
                            };
                        }
                        break;
                    }
                    case OIDType.NOM_ATTR_PT_NOTES1:
                    case OIDType.NOM_ATTR_PT_NOTES2:
                    {
                        var attributeValue = attribute.AttributeValue as IntellivueString;
                        if (attributeValue != null)
                        {
                            patientInfo ??= new PatientInfo(logSessionId);
                            patientInfo.Comment = !string.IsNullOrEmpty(patientInfo.Comment) 
                                ? patientInfo.Comment + ". " + attributeValue.Value 
                                : attributeValue.Value;
                        }
                        break;
                    }
                }
            }
        }

        return patientInfo != null;
    }

    private IEnumerable<PollMdiDataReply> GetPollResults(
        LinkedCommandMessageBundle messageBundle)
    {
        foreach (var message in messageBundle.Messages)
        {
            if (message is not DataExportCommandMessage dataExportMessage)
                continue;
            if (dataExportMessage.RemoteOperationData is not RemoteOperationResult remoteOperationResult)
                continue;
            if (remoteOperationResult.CommandType != DataExportCommandType.ConfirmedAction)
                continue;
            if (remoteOperationResult.Data is not ActionResultCommand actionResult)
                continue;
            if (actionResult.Data is not PollMdiDataReply pollResult)
                continue;
            yield return pollResult;
        }
    }

    private IEnumerable<AttributeValueAssertion> ExtractAttributes(
        PollMdiDataReply pollResult)
    {
        return from singleContextPoll in pollResult.PollContexts.Values
            from observation in singleContextPoll.Observations.Values
            from observationAttribute in observation.Attributes.Values
            select observationAttribute;
    }
}