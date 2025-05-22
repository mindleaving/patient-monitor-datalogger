using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models.Attributes;
using PatientMonitorDataLogger.Shared;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.DataProcessing;

public class PhilipsIntellivuePatientInfoExtractor
{
    public bool TryExtract(
        Guid logSessionId,
        ICommandMessage message,
        out PatientInfo? patientInfo)
    {
        patientInfo = null;
        if (message is not DataExportCommandMessage dataExportMessage)
            return false;
        if (dataExportMessage.RemoteOperationData is not RemoteOperationResult remoteOperationResult)
            return false;
        if (remoteOperationResult.CommandType != DataExportCommandType.ConfirmedAction)
            return false;
        if (remoteOperationResult.Data is not ActionResultCommand actionResult)
            return false;
        if (actionResult.Data is not PollMdiDataReply pollResult)
            return false;
        foreach (var singleContextPoll in pollResult.PollContexts.Values)
        {
            var contextId = singleContextPoll.ContextId;
            foreach (var observation in singleContextPoll.Observations.Values)
            {
                var handle = observation.Handle;
                foreach (var observationAttribute in observation.Attributes.Values)
                {
                    switch ((OIDType)observationAttribute.AttributeId)
                    {
                        case OIDType.NOM_ATTR_PT_NAME_GIVEN:
                        {
                            var attributeValue = observationAttribute.AttributeValue as IntellivueString;
                            if (attributeValue != null)
                            {
                                patientInfo ??= new PatientInfo(logSessionId);
                                patientInfo.FirstName = attributeValue.Value;
                            }
                            break;
                        }
                        case OIDType.NOM_ATTR_PT_NAME_FAMILY:
                        {
                            var attributeValue = observationAttribute.AttributeValue as IntellivueString;
                            if (attributeValue != null)
                            {
                                patientInfo ??= new PatientInfo(logSessionId);
                                patientInfo.LastName = attributeValue.Value;
                            }
                            break;
                        }
                        case OIDType.NOM_ATTR_PT_ID:
                        {
                            var attributeValue = observationAttribute.AttributeValue as IntellivueString;
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
                            var attributeValue = observationAttribute.AttributeValue as EnumAttributeValue<PatientSex>;
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
                            var attributeValue = observationAttribute.AttributeValue as IntellivueString;
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
        }
        return patientInfo != null;
    }
}