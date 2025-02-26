using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue;

public class CommandMessageCreator
{
    private ushort nextInvokeId = 0;
    private ushort nextPollNumber = 0;

    #region Association Control Protocol

    public AssociationCommandMessage CreateAssociationRequest(
        MdseUserInfoStd associationInfo)
    {
        var associationInfoBytes = associationInfo.Serialize();
        var userData = new AssociationRequestUserData(new(associationInfoBytes.Length), associationInfo);
        var userDataBytes = userData.Serialize();
        var presentationTrailer = PresentationTrailer.AssociationRequest;
        return BuildAssociationCommandMessage(
            AssociationCommandType.RequestAssociation,
            SessionData.AssociationRequest,
            PresentationHeader.BuildForAssociationRequest(userDataBytes.Length, presentationTrailer.Length),
            userData,
            presentationTrailer);
    }

    public AssociationCommandMessage CreateAssociationRequest(
        TimeSpan minimumPollPeriod,
        ExtendedPollProfileOptions pollOptions,
        StartupMode startupMode = StartupMode.ColdStart)
    {
        var extendedPollProfile = new ExtendedPollProfile(
            pollOptions, 
            new([]));
        var pollProfileSupport = new PollProfileSupport(
            PollProfileRevision.Revision0,
            new RelativeTime(minimumPollPeriod),
            2500, //PollProfileSupport.MaxAllowedRs232Mtu,
            PollProfileSupport.MaxAllowedRs232Mtu,
            uint.MaxValue,
            PollProfileOptions.P_OPT_DYN_CREATE_OBJECTS | PollProfileOptions.P_OPT_DYN_DELETE_OBJECTS,
            new(
            [
                new AttributeValueAssertion(
                    (ushort)ProtocolIdentification.NOM_ATTR_POLL_PROFILE_EXT,
                    extendedPollProfile)
            ]));
        var associationInfo = new MdseUserInfoStd(
            ProtocolVersion.Version1,
            NomenclatureVersion.Version,
            FunctionalUnits.None,
            SystemType.Client,
            startupMode,
            new([]),
            new(
            [
                new AttributeValueAssertion(
                    (ushort)ProtocolIdentification.NOM_POLL_PROFILE_SUPPORT,
                    pollProfileSupport)
            ]));
        return CreateAssociationRequest(associationInfo);
    }

    public AssociationCommandMessage CreateAssociationAccept(
        AssociationCommandMessage associationRequest)
    {
        var associationRequestUserData = (AssociationRequestUserData)associationRequest.UserData!;
        var requestedMdsUserInfo = associationRequestUserData.UserInfo;

        var presentationTrailer = PresentationTrailer.AssociationResponse;
        var userData = new MdseUserInfoStd(
            requestedMdsUserInfo.ProtocolVersion,
            requestedMdsUserInfo.NomenclatureVersion,
            requestedMdsUserInfo.FunctionalUnits,
            SystemType.Server,
            StartupMode.ColdStart,
            requestedMdsUserInfo.OptionList,
            requestedMdsUserInfo.SupportedApplicationProfiles);
        var userDataBytes = userData.Serialize();
        return BuildAssociationCommandMessage(
            AssociationCommandType.AssociationAccepted,
            SessionData.AssociationResponse,
            PresentationHeader.BuildForAssociationResponse(userDataBytes.Length, presentationTrailer.Length),
            userData,
            presentationTrailer);
    }

    public AssociationCommandMessage CreateAssociationRefusal()
    {
        return BuildAssociationCommandMessage(
            AssociationCommandType.Refuse,
            SessionData.AssociationRefuse,
            PresentationHeader.Empty,
            null,
            PresentationTrailer.AssociationRefuse);
    }

    public AssociationCommandMessage CreateAssociationReleaseRequest()
    {
        return BuildAssociationCommandMessage(
            AssociationCommandType.RequestRelease,
            SessionData.AssociationReleaseRequest,
            PresentationHeader.ReleaseRequest,
            null,
            PresentationTrailer.ReleaseRequest);
    }

    public AssociationCommandMessage CreateAssociationReleaseResponse()
    {
        return BuildAssociationCommandMessage(
            AssociationCommandType.Released,
            SessionData.AssociationReleaseResponse,
            PresentationHeader.ReleaseResponse,
            null,
            PresentationTrailer.ReleaseResponse);
    }

    public AssociationCommandMessage CreateAssociationAbort()
    {
        return BuildAssociationCommandMessage(
            AssociationCommandType.Abort,
            SessionData.AssociationAbort,
            PresentationHeader.AssociationAbort,
            null,
            PresentationTrailer.AssociationAbort);
    }

    private AssociationCommandMessage BuildAssociationCommandMessage(
        AssociationCommandType associationCommandType,
        SessionData sessionData,
        PresentationHeader presentationHeader,
        IAssociationCommandUserData? userData,
        PresentationTrailer presentationTrailer)
    {
        var userDataBytes = userData?.Serialize() ?? [];
        var sessionHeader = new SessionHeader(
            associationCommandType,
            (ushort)(sessionData.Length + presentationHeader.Length + userDataBytes.Length + presentationTrailer.Length));
        return new AssociationCommandMessage(
            sessionHeader,
            sessionData,
            presentationHeader,
            userData,
            presentationTrailer);
    }

    #endregion

    #region Data Export Protocol

    public DataExportCommandMessage CreateMdsCreateEvent(
        ushort presentationContextId,
        ushort invokeId,
        RelativeTime eventTime)
    {
        var systemTypeAttribute = new AttributeValueAssertion(
            (ushort)OIDType.NOM_ATTR_SYS_TYPE,
            new NomenclatureReference(NomenclaturePartition.NOM_PART_OBJ, (ushort)ObjectClass.NOM_MOC_VMO_AL_MON));
        var mdsCreateInfo = new MdsCreateInfo(
            new(OIDType.NOM_MOC_VMS_MDS, new(0, 0)),
            new(
            [
                systemTypeAttribute
            ]));
        var mdsCreateInfoBytes = mdsCreateInfo.Serialize();
        var eventReport = new EventReportCommand(
            new(OIDType.NOM_MOC_VMS_MDS, new(0, 0)),
            eventTime,
            OIDType.NOM_NOTI_MDS_CREAT,
            (ushort)(mdsCreateInfoBytes.Length),
            mdsCreateInfo);
        var eventReportBytes = eventReport.Serialize();
        var remoteOperationData = new RemoteOperationInvoke(
            invokeId,
            DataExportCommandType.ConfirmedEventReport,
            (ushort)(eventReportBytes.Length),
            eventReport);
        var remoteOperationDataBytes = remoteOperationData.Serialize();
        var remoteOperationHeader = new RemoteOperationHeader(RemoteOperationType.Invoke, (ushort)remoteOperationDataBytes.Length);
        return new DataExportCommandMessage(new SessionPresentationHeader(presentationContextId), remoteOperationHeader, remoteOperationData);
    }

    public DataExportCommandMessage CreateMdsCreateEventResult(
        ushort presentationContextId,
        ushort invokeId,
        ManagedObjectId eventReportManagedObject)
    {
        var eventReportResult = new EventReportResultCommand(
            eventReportManagedObject,
            new RelativeTime((uint)(DateTime.UtcNow.Ticks * 8)),
            OIDType.NOM_NOTI_MDS_CREAT,
            0,
            null);
        var eventReportResultBytes = eventReportResult.Serialize();
        var remoteOperationResult = new RemoteOperationResult(
            invokeId,
            DataExportCommandType.ConfirmedEventReport,
            (ushort)eventReportResultBytes.Length,
            eventReportResult);
        var remoteOperationResultBytes = remoteOperationResult.Serialize();
        var remoteOperationHeader = new RemoteOperationHeader(RemoteOperationType.Result, (ushort)remoteOperationResultBytes.Length);
        return new DataExportCommandMessage(
            new SessionPresentationHeader(presentationContextId),
            remoteOperationHeader,
            remoteOperationResult);
    }

    public DataExportCommandMessage CreateSinglePollRequest(
        ushort presentationContextId,
        NomenclatureReference pollObjectType,
        OIDType pollDataAttributeGroup)
    {
        var invokeId = nextInvokeId++;
        var pollMdiDataRequest = new PollMdiDataRequest(nextPollNumber++, pollObjectType, pollDataAttributeGroup);
        var actionManagedObject = new ManagedObjectId(OIDType.NOM_MOC_VMS_MDS, new(0, 0));
        var actionType = OIDType.NOM_ACT_POLL_MDIB_DATA;

        return CreateActionCommandMessage(
            presentationContextId,
            invokeId,
            actionManagedObject,
            actionType,
            pollMdiDataRequest);
    }

    public DataExportCommandMessage CreatePollReply(
        ushort presentationContextId,
        ushort invokeId,
        PollMdiDataRequest pollRequest,
        RelativeTime currentTime,
        Models.List<ObservationPoll> observations)
    {
        var pollReply = new PollMdiDataReply(
            pollRequest.PollNumber,
            currentTime,
            AbsoluteTime.Invalid, // According to the manual, the monitor doesn't support absolute time
            pollRequest.ObjectType,
            pollRequest.AttributeGroup,
            new(
            [
                new(1, observations)
            ]));
        var pollReplyBytes = pollReply.Serialize();
        var resultData = new ActionResultCommand(
            new(OIDType.NOM_MOC_VMS_MDS, new(0, 0)),
            OIDType.NOM_ACT_POLL_MDIB_DATA,
            (ushort)pollReplyBytes.Length,
            pollReply);
        var resultDataBytes = resultData.Serialize();
        var remoteOperationResult = new RemoteOperationResult(invokeId, DataExportCommandType.ConfirmedAction, (ushort)resultDataBytes.Length, resultData);
        var remoteOperationResultBytes = remoteOperationResult.Serialize();
        return new DataExportCommandMessage(
            new SessionPresentationHeader(presentationContextId),
            new RemoteOperationHeader(RemoteOperationType.Result, (ushort)remoteOperationResultBytes.Length),
            remoteOperationResult);
    }

    public DataExportCommandMessage CreateExtendedPollRequest(
        ushort presentationContextId,
        NomenclatureReference pollObjectType,
        OIDType pollDataAttributeGroup,
        TimeSpan? pollPeriod = null,
        ushort? numberOfPrioritizedObjects = null)
    {
        var invokeId = nextInvokeId++;
        var extendedPollMdiDataRequest = new ExtendedPollMdiDataRequest(nextPollNumber++, pollObjectType, pollDataAttributeGroup, new([]));
        if(pollPeriod.HasValue)
            extendedPollMdiDataRequest.SetTimePeriodicDataPoll(pollPeriod.Value);
        if(numberOfPrioritizedObjects.HasValue)
            extendedPollMdiDataRequest.SetNumberOfPrioritizedObjects(numberOfPrioritizedObjects.Value);
        var actionManagedObject = new ManagedObjectId(OIDType.NOM_MOC_VMS_MDS, new(0, 0));
        var actionType = OIDType.NOM_ACT_POLL_MDIB_DATA_EXT;

        return CreateActionCommandMessage(
            presentationContextId,
            invokeId,
            actionManagedObject,
            actionType,
            extendedPollMdiDataRequest);
    }

    public DataExportCommandMessage CreateExtendedPollReply(
        ushort presentationContextId,
        ushort invokeId,
        ushort sequenceNumber,
        ExtendedPollMdiDataRequest pollRequest,
        RelativeTime currentTime,
        Models.List<ObservationPoll> observations)
    {
        var pollReply = new ExtendedPollMdiDataReply(
            pollRequest.PollNumber,
            sequenceNumber,
            currentTime,
            AbsoluteTime.Invalid, // According to the manual, the monitor doesn't support absolute time
            pollRequest.ObjectType,
            pollRequest.AttributeGroup,
            new(
            [
                new(1, observations)
            ]));
        var pollReplyBytes = pollReply.Serialize();
        var resultData = new ActionResultCommand(
            new(OIDType.NOM_MOC_VMS_MDS, new(0, 0)),
            OIDType.NOM_ACT_POLL_MDIB_DATA_EXT,
            (ushort)pollReplyBytes.Length,
            pollReply);
        var resultDataBytes = resultData.Serialize();
        var remoteOperationResult = new RemoteOperationResult(invokeId, DataExportCommandType.ConfirmedAction, (ushort)resultDataBytes.Length, resultData);
        var remoteOperationResultBytes = remoteOperationResult.Serialize();
        return new DataExportCommandMessage(
            new SessionPresentationHeader(presentationContextId),
            new RemoteOperationHeader(RemoteOperationType.Result, (ushort)remoteOperationResultBytes.Length),
            remoteOperationResult);
    }

    public DataExportCommandMessage CreateKeepAliveMessage(
        ushort presentationContextId)
    {
        // As suggested by the manual, we poll the alerts object with the VMO Static Context attribute group,
        // as it has as little processing overhead as possible.
        return CreateSinglePollRequest(presentationContextId, PollObjectTypes.Alerts, PollAttributeGroups.Alerts.VmoStaticContext);
    }

    public DataExportCommandMessage CreateGetPriorityListRequest(
        ushort presentationContextId,
        params OIDType[] listIds)
    {
        var getCommand = new GetCommand(new(OIDType.NOM_MOC_VMS_MDS, new(0, 0)), new(listIds));
        var getCommandBytes = getCommand.Serialize();
        var remoteOperationInvoke = new RemoteOperationInvoke(
            nextInvokeId++,
            DataExportCommandType.Get,
            (ushort)getCommandBytes.Length,
            getCommand);
        var remoteOperationInvokeBytes = remoteOperationInvoke.Serialize();
        var remoteOperationHeader = new RemoteOperationHeader(RemoteOperationType.Invoke, (ushort)remoteOperationInvokeBytes.Length);
        return new(new(presentationContextId), remoteOperationHeader, remoteOperationInvoke);
    }

    private static DataExportCommandMessage CreateActionCommandMessage(
        ushort presentationContextId,
        ushort invokeId,
        ManagedObjectId actionManagedObject,
        OIDType actionType,
        IActionData actionData)
    {
        var pollMdiDataRequestBytes = actionData.Serialize();
        var actionCommnad = new ActionCommand(
            actionManagedObject,
            actionType,
            (ushort)pollMdiDataRequestBytes.Length,
            actionData);
        var actionCommandBytes = actionCommnad.Serialize();

        var remoteOperationInvoke = new RemoteOperationInvoke(
            invokeId,
            DataExportCommandType.ConfirmedAction,
            (ushort)actionCommandBytes.Length,
            actionCommnad);
        var remoteOperationInvokeBytes = remoteOperationInvoke.Serialize();
        var remoteOperationHeader = new RemoteOperationHeader(RemoteOperationType.Invoke, (ushort)remoteOperationInvokeBytes.Length);

        return new DataExportCommandMessage(
            new(presentationContextId),
            remoteOperationHeader,
            remoteOperationInvoke);
    }

    #endregion
}