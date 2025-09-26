import * as Enums from './enums';

export namespace Models {
    interface Alert {
        timestamp: Date;
        parameterName: string;
        text: string;
    }
    interface IInfusionPumpInfo extends Models.IMedicalDeviceInfo {
        infusionPumpType: Enums.InfusionPumpType;
    }
    interface ILogSessionData {
        logSessionId: string;
    }
    interface IMedicalDeviceDataSettings {
        deviceType: Enums.MedicalDeviceType;
    }
    interface IMedicalDeviceInfo {
        deviceType: Enums.MedicalDeviceType;
    }
    interface IMedicalDeviceSettings {
        deviceType: Enums.MedicalDeviceType;
    }
    interface IMonitorData {
        type: Enums.MonitorDataType;
    }
    interface InfusionPumpDataSettings extends Models.IMedicalDeviceDataSettings {
        
    }
    interface InfusionPumpParameter {
        name: string;
        value: string;
    }
    interface InfusionPumpSettings extends Models.IMedicalDeviceSettings {
        infusionPumpType: Enums.InfusionPumpType;
    }
    interface InfusionPumpState {
        timestamp: Date;
        pumpIndex: Models.PumpIndex;
        parameters: Models.InfusionPumpParameter[];
    }
    interface IODevice extends System.IDisposable {
        
    }
    interface IPatientMonitorInfo extends Models.IMedicalDeviceInfo {
        monitorType: Enums.PatientMonitorType;
    }
    interface ISerializable {
        
    }
    interface LogSessionEvent {
        timestamp?: Date | null;
        message: string;
    }
    interface NumericsData extends Models.IMonitorData {
        timestamp: Date;
        values: { [key: string]: Models.NumericsValue };
    }
    interface NumericsValue {
        timestamp: Date;
        value: number;
        unit: string;
        state: Enums.MeasurementState;
    }
    interface PatientInfo extends Models.ILogSessionData {
        patientId: string;
        encounterId: string;
        firstName: string;
        lastName: string;
        sex?: Enums.Sex | null;
        dateOfBirth?: Date | null;
        comment: string;
    }
    interface PatientMonitorDataSettings extends Models.IMedicalDeviceDataSettings {
        includeAlerts: boolean;
        includeNumerics: boolean;
        includeWaves: boolean;
        includePatientInfo: boolean;
        selectedNumericsTypes: string[];
        selectedWaveTypes: Enums.WaveType[];
    }
    interface PatientMonitorSettings extends Models.IMedicalDeviceSettings {
        monitorType: Enums.PatientMonitorType;
    }
    interface WaveData extends Models.IMonitorData {
        measurementType: string;
        timestampFirstDataPoint: Date;
        sampleRate: number;
        values: number[];
    }
    interface AbsoluteTime extends Models.ISerializable {
        century: number;
        year: number;
        month: number;
        day: number;
        hour: number;
        minute: number;
        second: number;
        milliseconds: number;
    }
    interface ActionCommand extends Models.IRemoteOperationInvokeData {
        managedObject: Models.ManagedObjectId;
        scope: number;
        actionType: Enums.OIDType;
        length: number;
        data: Models.IActionData;
    }
    interface ActionResultCommand extends Models.IRemoteOperationResultData, Models.IRemoteOperationErrorData {
        managedObject: Models.ManagedObjectId;
        actionType: Enums.OIDType;
        length: number;
        data: Models.IActionResultData;
    }
    interface ASNLength {
        length: number;
    }
    interface Association {
        presentationContextId: number;
    }
    interface AssociationCommandMessage extends Models.ICommandMessage {
        sessionHeader: Models.SessionHeader;
        sessionData: Models.SessionData;
        presentationHeader: Models.PresentationHeader;
        userData?: Models.IAssociationCommandUserData;
        presentationTrailer: Models.PresentationTrailer;
    }
    interface AssociationRequestUserData extends Models.IAssociationCommandUserData {
        length: Models.ASNLength;
        userInfo: Models.MdseUserInfoStd;
    }
    interface AttributeContext {
        messageType: Enums.CommandMessageType;
    }
    interface AttributeIdList extends Models.ISerializable {
        count: number;
        length: number;
        values: Enums.OIDType[];
    }
    interface AttributeModification extends Models.ISerializable {
        modifyOperator: Enums.ModifyOperator;
        attribute: Models.AttributeValueAssertion;
    }
    interface AttributeValueAssertion extends Models.ISerializable {
        attributeId: number;
        length: number;
        attributeValue: Models.ISerializable;
    }
    interface DataExportCommandMessage extends Models.ICommandMessage {
        sessionPresentationHeader: Models.SessionPresentationHeader;
        remoteOperationHeader: Models.RemoteOperationHeader;
        remoteOperationData: Models.IRemoteOperationResult;
    }
    interface EventReportCommand extends Models.IRemoteOperationInvokeData {
        managedObject: Models.ManagedObjectId;
        eventTime: Models.RelativeTime;
        eventType: Enums.OIDType;
        length: number;
        data: Models.IEventReportData;
    }
    interface EventReportResultCommand extends Models.IRemoteOperationResultData {
        managedObject: Models.ManagedObjectId;
        currentTime: Models.RelativeTime;
        eventType: Enums.OIDType;
        length: number;
        data?: Models.IEventReportResultData;
    }
    interface ExtendedPollMdiDataReply extends Models.PollMdiDataReply {
        sequenceNumber: number;
    }
    interface ExtendedPollMdiDataRequest extends Models.IActionData {
        pollNumber: number;
        objectType: Models.NomenclatureReference;
        attributeGroup: Enums.OIDType;
        attributes: Models.List<Models.AttributeValueAssertion>;
    }
    interface ExtendedPollProfile extends Models.ISerializable {
        options: Enums.ExtendedPollProfileOptions;
        extendedAttributes: Models.List<Models.AttributeValueAssertion>;
    }
    interface GetCommand extends Models.IRemoteOperationInvokeData {
        managedObject: Models.ManagedObjectId;
        scope: number;
        attributeIdList: Models.AttributeIdList;
    }
    interface GetError extends Models.ISerializable {
        errorStatus: Enums.ErrorStatus;
        attributeId: Enums.OIDType;
    }
    interface GetListError extends Models.IRemoteOperationErrorData {
        managedObject: Models.ManagedObjectId;
        errors: Models.List<Models.GetError>;
    }
    interface GetResultCommand extends Models.IRemoteOperationResultData {
        managedObject: Models.ManagedObjectId;
        attributeList: Models.List<Models.AttributeValueAssertion>;
    }
    interface GlobalHandle extends Models.ISerializable {
        contextId: number;
        handle: number;
    }
    interface Handle extends Models.ISerializable {
        id: number;
    }
    interface IActionData extends Models.ISerializable {
        
    }
    interface IActionResultData extends Models.ISerializable {
        
    }
    interface IAssociationCommandUserData extends Models.ISerializable {
        
    }
    interface ICommandData extends Models.ISerializable {
        
    }
    interface ICommandMessage extends Models.ISerializable {
        messageType: Enums.CommandMessageType;
    }
    interface IEventReportData extends Models.ISerializable {
        
    }
    interface IEventReportResultData extends Models.ISerializable {
        
    }
    interface IntellivueFloat extends Models.ISerializable {
        value: number;
    }
    interface IntellivueString extends Models.ISerializable {
        value: string;
    }
    interface InvalidObjectInstanceError extends Models.IRemoteOperationErrorData {
        managedObject: Models.ManagedObjectId;
    }
    interface InvalidScopeError extends Models.IRemoteOperationErrorData {
        scope: number;
    }
    interface IRemoteOperation extends Models.IRemoteOperationResult {
        commandType: Enums.DataExportCommandType;
    }
    interface IRemoteOperationErrorData extends Models.ISerializable {
        
    }
    interface IRemoteOperationInvokeData extends Models.ICommandData {
        
    }
    interface IRemoteOperationResult extends Models.ISerializable {
        invokeId: number;
        length: number;
    }
    interface IRemoteOperationResultData extends Models.ICommandData {
        
    }
    interface LengthIndicator {
        length: number;
    }
    interface LinkedCommandMessageBundle {
        messages: Models.ICommandMessage[];
    }
    interface List<T> extends Models.ISerializable {
        count: number;
        length: number;
        values: T[];
    }
    interface ManagedObjectId extends Models.ISerializable {
        objectClass: Enums.OIDType;
        objectInstance: Models.GlobalHandle;
    }
    interface MdsCreateInfo extends Models.IEventReportData {
        managedObject: Models.ManagedObjectId;
        attributeList: Models.List<Models.AttributeValueAssertion>;
    }
    interface MdseUserInfoStd extends Models.IAssociationCommandUserData {
        protocolVersion: Enums.ProtocolVersion;
        nomenclatureVersion: Enums.NomenclatureVersion;
        functionalUnits: Enums.FunctionalUnits;
        systemType: Enums.SystemType;
        startupMode: Enums.StartupMode;
        optionList: Models.List<Models.AttributeValueAssertion>;
        supportedApplicationProfiles: Models.List<Models.AttributeValueAssertion>;
    }
    interface NomenclatureReference extends Models.ISerializable, System.IEquatable<Models.NomenclatureReference> {
        partition: Enums.NomenclaturePartition;
        code: number;
    }
    interface NoSuchActionError extends Models.IRemoteOperationErrorData {
        objectClassId: Enums.ObjectClass;
        actionType: Enums.OIDType;
    }
    interface NoSuchObjectClassError extends Models.IRemoteOperationErrorData {
        objectClass: Enums.OIDType;
    }
    interface NoSuchObjectInstanceError extends Models.IRemoteOperationErrorData {
        managedObject: Models.ManagedObjectId;
    }
    interface ObservationPoll extends Models.ISerializable {
        handle: number;
        attributes: Models.List<Models.AttributeValueAssertion>;
    }
    interface PhilipsIntellivueFrame extends Models.ISerializable {
        header: Models.PhilipsIntellivueFrameHeader;
        userData: Models.ICommandMessage;
        checksum: number;
    }
    interface PhilipsIntellivueFrameHeader extends Models.ISerializable {
        protocolId: Enums.ProtocolId;
        messageType: Enums.MessageType;
        userDataLength: number;
    }
    interface PhilipsIntellivuePatientMonitorInfo extends Models.IPatientMonitorInfo {
        name: string;
    }
    interface PhilipsIntellivueSettings extends Models.PatientMonitorSettings {
        serialPortName: string;
        serialPortBaudRate: number;
    }
    interface PollAttributeGroups {
        
    }
    interface PollDataRequestPeriod extends Models.ISerializable {
        activePeriod: Models.RelativeTime;
    }
    interface PollMdiDataReply extends Models.IActionResultData {
        pollNumber: number;
        relativeTimeStamp: Models.RelativeTime;
        absoluteTimeStamp: Models.AbsoluteTime;
        objectType: Models.NomenclatureReference;
        attributeGroup: Enums.OIDType;
        pollContexts: Models.List<Models.SingleContextPoll>;
    }
    interface PollMdiDataRequest extends Models.IActionData {
        pollNumber: number;
        objectType: Models.NomenclatureReference;
        attributeGroup: Enums.OIDType;
    }
    interface PollObjectTypes {
        
    }
    interface PollProfileSupport extends Models.ISerializable {
        profileRevision: Enums.PollProfileRevision;
        minimumPollPeriod: Models.RelativeTime;
        maxMtuRx: number;
        maxMtuTx: number;
        maxTransmitBandwidth: number;
        options: Enums.PollProfileOptions;
        optionalPackages: Models.List<Models.AttributeValueAssertion>;
    }
    interface PresentationHeader {
        payload: number[];
        length: number;
    }
    interface PresentationTrailer {
        payload: number[];
        length: number;
    }
    interface ProcessingFailure extends Models.IRemoteOperationErrorData {
        errorId: Enums.OIDType;
        length: number;
        additionalInformation: number[];
    }
    interface RelativeTime extends Models.ISerializable {
        ticks: number;
        totalSeconds: number;
    }
    interface RemoteOperationError extends Models.IRemoteOperationResult {
        errorValue: Enums.RemoteOperationErrorType;
        data: Models.IRemoteOperationErrorData;
    }
    interface RemoteOperationHeader extends Models.ISerializable {
        type: Enums.RemoteOperationType;
        length: number;
    }
    interface RemoteOperationInvoke extends Models.IRemoteOperation {
        data: Models.IRemoteOperationInvokeData;
    }
    interface RemoteOperationLinkedResult extends Models.RemoteOperationResult {
        linkedId: Models.RemoteOperationLinkedResultId;
    }
    interface RemoteOperationLinkedResultId extends Models.ISerializable {
        state: Enums.RemoteOperationLinkedResultState;
        count: number;
    }
    interface RemoteOperationResult extends Models.IRemoteOperation {
        data: Models.IRemoteOperationResultData;
    }
    interface SessionData {
        payload: number[];
        length: number;
    }
    interface SessionHeader {
        type: Enums.AssociationCommandType;
        length: Models.LengthIndicator;
    }
    interface SessionPresentationHeader extends Models.ISerializable {
        sessionId: number;
        presentationContextId: number;
    }
    interface SetCommand extends Models.IRemoteOperationInvokeData {
        managedObject: Models.ManagedObjectId;
        scope: number;
        modifications: Models.List<Models.AttributeModification>;
    }
    interface SetError extends Models.ISerializable {
        errorStatus: Enums.ErrorStatus;
        modifyOperator: Enums.ModifyOperator;
        attributeId: Enums.OIDType;
    }
    interface SetListError extends Models.IRemoteOperationErrorData {
        managedObject: Models.ManagedObjectId;
        errors: Models.List<Models.SetError>;
    }
    interface SetResultCommand extends Models.IRemoteOperationResultData {
        managedObject: Models.ManagedObjectId;
        attributeList: Models.List<Models.AttributeValueAssertion>;
    }
    interface SimulatedPhilipsIntellivueSettings extends Models.PatientMonitorSettings {
        
    }
    interface SingleContextPoll extends Models.ISerializable {
        contextId: number;
        observations: Models.List<Models.ObservationPoll>;
    }
    interface TextId extends Models.ISerializable {
        label: Enums.Labels;
    }
    interface UnitCodesMap {
        
    }
    interface WaveDefinition {
        description: string;
        label: Enums.Labels;
        observedValue: Enums.SCADAType;
        units: Enums.UnitCodes[];
    }
    interface Waves {
        
    }
    interface Numerics {
        
    }
    interface Waves {
        
    }
    interface Enumerations {
        
    }
    interface System {
        
    }
    interface Alerts {
        
    }
    interface PatientDemographics {
        
    }
    interface GEDashPatientMonitorInfo extends Models.IPatientMonitorInfo {
        name: string;
    }
    interface GEDashSettings extends Models.PatientMonitorSettings {
        serialPortName: string;
        serialPortBaudRate: number;
    }
    interface BBraunInfusionPumpInfo extends Models.IInfusionPumpInfo {
        bedId: string;
    }
    interface BBraunInfusionPumpSettings extends Models.InfusionPumpSettings {
        hostname: string;
        port: number;
        useCharacterStuffing: boolean;
        pollPeriod: string;
    }
    interface BBraunRackConfiguration {
        rackCountPillar1: number;
        rackCountPillar2: number;
        rackCountPillar3: number;
    }
    interface BBraunUnitMap {
        
    }
    interface InfusionPumpParameters {
        pumpIndex: Models.PumpIndex;
        name: string;
        model: string;
        rateInMilliliterPerHour?: number | null;
        volumeToBeInfusedInMilliliter?: number | null;
        medicationLongName: string;
        medicationShortName: string;
        medicationId: string;
        sizeOfSyringeInMilliliter?: number | null;
        remainingVolumeInSyringeInMilliliter?: number | null;
        remainingTimeOrNextPreAlarmInSeconds?: number | null;
        volumeInfusedInMilliliter?: number | null;
        infusionTimeInMinutes?: number | null;
        batteryTimeInMinutes?: number | null;
        remainingStandbyTimeInMinutes?: number | null;
        rateOfBolusInMilliliterPerHour?: number | null;
        bolusVolumeDeliveredInMilliliter?: number | null;
        isDoseModeActive?: boolean | null;
        drugConcentration?: number | null;
        drugConcentrationUnit?: number | null;
        doseRate?: number | null;
        doseRateUnit?: number | null;
        actualPressureSetting?: number | null;
        actualPressureInPercentOfMax?: number | null;
        isReadyForInfusion?: boolean | null;
        isBolusActive?: boolean | null;
        isActive?: boolean | null;
        usesCCFunction?: boolean | null;
        isBolusFunctionReleased?: boolean | null;
        isInStandby?: boolean | null;
        isDataLockOn?: boolean | null;
        powerSource?: Enums.BccDevicePowerSource | null;
    }
    interface PumpIndex extends Models.ISerializable, System.IEquatable<Models.PumpIndex>, System.IComparable<Models.PumpIndex> {
        pillar: number;
        slot: number;
        slotCharacter: string;
    }
    interface Quadruple extends Models.ISerializable {
        relativeTimeInSeconds: number;
        address: Models.PumpIndex;
        parameter: string;
        value?: string;
    }
    interface RackParameters {
        numberOfConnectedPillars: number;
        lastScannedBarcode?: string;
        rackConfiguration: Models.BBraunRackConfiguration;
        serialNumber: number;
        softwareVersion: string;
    }
    interface SimulatedBBraunInfusionPumpSettings extends Models.InfusionPumpSettings {
        bedId: string;
        pillarCount: number;
        pumpCount: number;
        pollPeriod: string;
    }
    interface DataWriterSettings {
        outputDirectory: string;
    }
    interface LogSession extends System.IDisposable {
        id: string;
        settings: Models.LogSessionSettings;
        patientInfo?: Models.PatientInfo;
        latestObservations: { [key: string]: Models.DataExport.Observation };
        shouldBeRunning: boolean;
        status: Models.LogStatus;
    }
    interface LogSessionSettings {
        name: string;
        deviceSettings: Models.IMedicalDeviceSettings;
        dataSettings: Models.IMedicalDeviceDataSettings;
        csvSeparator: string;
    }
    interface LogStatus extends Models.ILogSessionData {
        isRunning: boolean;
        deviceInfo: Models.IMedicalDeviceInfo;
        startTime?: Date | null;
        recordedParameters: string[];
    }
    export namespace Attributes {
        interface AlarmMonitorGeneralInfo extends Models.ISerializable {
            alarmInstanceNumber: number;
            alarmText: Models.TextId;
            alertPriority: number;
            flags: Enums.AlertFlags;
        }
        interface AlarmMonitorStringInfo extends Models.Attributes.AlarmMonitorGeneralInfo {
            string: Models.IntellivueString;
        }
        interface Attributes {
            
        }
        interface CalibrationSpecification extends Models.ISerializable {
            lowerAbsoluteValue: Models.IntellivueFloat;
            upperAbsoluteValue: Models.IntellivueFloat;
            lowerScaledValue: number;
            upperScaledValue: number;
            increment: number;
            calibrationType: Enums.CalibrationType;
        }
        interface DeviceAlarmEntry extends Models.ISerializable {
            source: Enums.OIDType;
            code: Enums.OIDType;
            type: Enums.AlertType;
            state: Enums.AlertState;
            object: Models.ManagedObjectId;
            additionalInfoType: Enums.AlertInfoType;
            length: number;
            additionalInfo: Models.Attributes.AlarmMonitorGeneralInfo;
        }
        interface DeviceAlertCondition extends Models.ISerializable {
            deviceAlertState: Enums.AlertState;
            changeCounter: number;
            maximumPhysiologicalAlarm: Enums.AlertType;
            maximumTechnicalAlarm: Enums.AlertType;
            maximumAuditoryAlarm: Enums.AlertType;
        }
        interface DisplayResolution extends Models.ISerializable {
            digitsBeforeDecimalPoint: number;
            digitsAfterDecimalPoint: number;
        }
        interface EnumAttributeValue<T> extends Models.ISerializable {
            value: T;
        }
        interface EnumObjectIdValue extends Models.ISerializable {
            objectId: Enums.OIDType;
            numericValue: Models.IntellivueFloat;
            unitCode: Enums.OIDType;
        }
        interface EnumObservationValue extends Models.ISerializable {
            physioId: Enums.SCADAType;
            state: Enums.MeasurementState;
            value: Models.Attributes.EnumValue;
        }
        interface EnumValue extends Models.ISerializable {
            choice: Enums.EnumUnionChoice;
            length: number;
            enumObjectId?: Enums.OIDType | null;
            enumObjectIdValue: Models.Attributes.EnumObjectIdValue;
        }
        interface MetricSpecification extends Models.ISerializable {
            updatePeriod: Models.RelativeTime;
            category: Enums.MetricCategory;
            access: Enums.MetricAccess;
            structure: Models.Attributes.MetricStructure;
            relevance: Enums.MetricRelevance;
        }
        interface MetricStructure extends Models.ISerializable {
            type: Enums.MetricStructureType;
            componentCount: number;
        }
        interface NumericObservedValue extends Models.ISerializable {
            physioId: Enums.SCADAType;
            state: Enums.MeasurementState;
            unitCode: Enums.UnitCodes;
            value: Models.IntellivueFloat;
        }
        interface ObservedValueArray extends Models.ISerializable {
            values: number[];
        }
        interface PatientMeasure extends Models.ISerializable {
            value: Models.IntellivueFloat;
            unit: Enums.UnitCodes;
        }
        interface SampleArrayFixedValueSpecificationEntry extends Models.ISerializable {
            fixedValueId: Enums.SampleArrayFixedValueId;
            fixedValue: number;
        }
        interface SampleArrayObservedValue extends Models.ISerializable {
            physioId: Enums.SCADAType;
            state: Enums.MeasurementState;
            values: Models.Attributes.ObservedValueArray;
        }
        interface SampleArraySpecifications extends Models.ISerializable {
            arraySize: number;
            sampleType: Models.Attributes.SampleType;
            flags: Enums.SampleArrayFlags;
        }
        interface SampleType extends Models.ISerializable {
            sampleSize: number;
            significantBits: number;
        }
        interface ScaleAndRangeSpecification extends Models.ISerializable {
            lowerAbsoluteValue: Models.IntellivueFloat;
            upperAbsoluteValue: Models.IntellivueFloat;
            lowerScaledValue: number;
            upperScaledValue: number;
        }
        interface ScaledRange extends Models.ISerializable {
            lowerScaledValue: number;
            upperScaledValue: number;
        }
        interface SystemLocalization extends Models.ISerializable {
            textCatalogRevision: number;
            language: Enums.Language;
            format: Enums.StringFormat;
        }
        interface SystemModel extends Models.ISerializable {
            manufacturer: number[];
            modelNumber: number[];
        }
        interface SystemSpecificationEntry extends Models.ISerializable {
            componentCapabilityId: number;
            length: number;
            values: number[];
        }
        interface UnknownAttributeValue extends Models.ISerializable {
            data: number[];
        }
        interface UshortAttributeValue extends Models.ISerializable {
            value: number;
        }
        interface VisualGridEntry extends Models.ISerializable {
            absoluteValue: Models.IntellivueFloat;
            scaledValue: number;
            level: number;
        }
        interface Numerics {
            
        }
        interface Waves {
            
        }
        interface Enumeration {
            
        }
        interface System {
            
        }
        interface Alerts {
            
        }
        interface PatientDemographics {
            
        }
    }
    export namespace RequestBodies {
        interface CopyDataToUsbDriveRequest {
            logSessionId: string;
            usbDrivePath: string;
        }
    }
    export namespace DataExport {
        interface LogSessionObservations extends Models.ILogSessionData {
            timestamp: Date;
            observations: Models.DataExport.Observation[];
        }
        interface Observation {
            timestamp: Date;
            parameterName: string;
            value: string;
            unit?: string;
        }
        interface UsbDriveInfo {
            path: string;
            name?: string;
        }
    }
}
export namespace System {
    interface IDisposable {
        
    }
    interface IEquatable<T> {
        
    }
    interface IComparable<T> {
        
    }
}
