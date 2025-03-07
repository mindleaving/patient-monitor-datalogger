import * as Enums from './enums';

export namespace Models {
    interface IInfusionPumpSettings extends Models.IMedicalDeviceSettings {
        infusionPumpType: Enums.InfusionPumpType;
    }
    interface IMedicalDeviceDataSettings {
        deviceType: Enums.MedicalDeviceType;
    }
    interface IMedicalDeviceSettings {
        deviceType: Enums.MedicalDeviceType;
    }
    interface InfusionPumpSettings extends Models.IInfusionPumpSettings {
        
    }
    interface IODevice extends System.IDisposable {
        
    }
    interface IPatientMonitorSettings extends Models.IMedicalDeviceSettings {
        monitorType: Enums.PatientMonitorType;
    }
    interface ISerializable {
        
    }
    interface PatientMonitorDataSettings extends Models.IMedicalDeviceDataSettings {
        includeAlerts: boolean;
        includeNumerics: boolean;
        includeWaves: boolean;
        includePatientInfo: boolean;
        selectedNumericsTypes: string[];
        selectedWaveTypes: Enums.WaveType[];
    }
    interface PatientMonitorSettings extends Models.IPatientMonitorSettings {
        
    }
    interface AbsoluteTime extends Models.ISerializable {
        century: System.Byte;
        year: System.Byte;
        month: System.Byte;
        day: System.Byte;
        hour: System.Byte;
        minute: System.Byte;
        second: System.Byte;
        milliseconds: System.Byte;
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
    interface AssociationCommandMessage extends Models.ICommandMessage {
        sessionHeader: Models.SessionHeader;
        sessionData: Models.SessionData;
        presentationHeader: Models.PresentationHeader;
        userData: Models.IAssociationCommandUserData;
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
        data: Models.IEventReportResultData;
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
    interface FrameAbortException extends System.Exception {
        
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
        value: System.Single;
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
    interface PhilipsIntellivuePatientMonitorSettings extends Models.PatientMonitorSettings {
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
        payload: System.Byte[];
        length: number;
    }
    interface PresentationTrailer {
        payload: System.Byte[];
        length: number;
    }
    interface ProcessingFailure extends Models.IRemoteOperationErrorData {
        errorId: Enums.OIDType;
        length: number;
        additionalInformation: System.Byte[];
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
        count: System.Byte;
    }
    interface RemoteOperationResult extends Models.IRemoteOperation {
        data: Models.IRemoteOperationResultData;
    }
    interface SessionData {
        payload: System.Byte[];
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
    interface SimulatedPhilipsIntellivuePatientMonitorSettings extends Models.PatientMonitorSettings {
        
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
    interface GEDashPatientMonitorSettings extends Models.PatientMonitorSettings {
        serialPortName: string;
        serialPortBaudRate: number;
    }
    interface BBraunInfusionPumpSettings extends Models.InfusionPumpSettings {
        hostname: string;
        port?: number | null;
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
        drugConcentrationUnit?: System.Byte | null;
        doseRate?: number | null;
        doseRateUnit?: System.Byte | null;
        actualPressureSetting?: System.Byte | null;
        actualPressureInPercentOfMax?: System.Byte | null;
        isReadyForInfusion?: boolean | null;
        isBolusActive?: boolean | null;
        isActive?: boolean | null;
        usesCCFunction?: boolean | null;
        isBolusFunctionReleased?: boolean | null;
        isInStandby?: boolean | null;
        isDataLockOn?: boolean | null;
        powerSource?: Enums.BccDevicePowerSource | null;
    }
    interface PumpIndex extends Models.ISerializable, System.IEquatable<Models.PumpIndex> {
        pillar: number;
        slot: number;
        slotCharacter: string;
    }
    interface Quadruple extends Models.ISerializable {
        relativeTimeInSeconds: number;
        address: Models.PumpIndex;
        parameter: string;
        value: string;
    }
    interface RackParameters {
        numberOfConnectedPillars: number;
        lastScannedBarcode: string;
        rackConfiguration: Models.BBraunRackConfiguration;
        serialNumber: number;
        softwareVersion: string;
    }
    interface DataWriterSettings {
        outputDirectory: string;
    }
    interface GEDashPatientMonitorInfo extends Models.IPatientMonitorInfo {
        name: string;
    }
    interface IInfusionPumpInfo extends Models.IMedicalDeviceInfo {
        infusionPumpType: Enums.InfusionPumpType;
    }
    interface BBraunInfusionPumpInfo extends Models.IInfusionPumpInfo {
        bedId: string;
    }
    interface ILogSessionData {
        logSessionId: string;
    }
    interface IMedicalDeviceInfo {
        deviceType: Enums.MedicalDeviceType;
    }
    interface IPatientMonitorInfo extends Models.IMedicalDeviceInfo {
        monitorType: Enums.PatientMonitorType;
    }
    interface LogSession extends System.IDisposable, System.IAsyncDisposable {
        id: string;
        settings: Models.LogSessionSettings;
        patientInfo: Models.PatientInfo;
        latestObservations: { [key: string]: Models.DataExport.Observation };
        shouldBeRunning: boolean;
        status: Models.LogStatus;
    }
    interface LogSessionSettings {
        name: string;
        deviceType: Enums.MedicalDeviceType;
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
    interface PatientInfo extends Models.ILogSessionData {
        patientId: string;
        encounterId: string;
        firstName: string;
        lastName: string;
        sex?: Enums.Sex | null;
        dateOfBirth?: Date | null;
        comment: string;
    }
    interface PhilipsIntellivuePatientMonitorInfo extends Models.IPatientMonitorInfo {
        name: string;
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
            digitsBeforeDecimalPoint: System.Byte;
            digitsAfterDecimalPoint: System.Byte;
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
            componentCount: System.Byte;
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
            sampleSize: System.Byte;
            significantBits: System.Byte;
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
            manufacturer: System.Byte[];
            modelNumber: System.Byte[];
        }
        interface SystemSpecificationEntry extends Models.ISerializable {
            componentCapabilityId: number;
            length: number;
            values: number[];
        }
        interface UnknownAttributeValue extends Models.ISerializable {
            data: System.Byte[];
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
        interface IMonitorData {
            type: Enums.MonitorDataType;
        }
        interface InfusionPumpParameter {
            name: string;
            value: string;
        }
        interface InfusionPumpState {
            timestamp: Date;
            pumpIndex: Models.PumpIndex;
            parameters: Models.DataExport.InfusionPumpParameter[];
        }
        interface LogSessionObservations extends Models.ILogSessionData {
            timestamp: Date;
            observations: Models.DataExport.Observation[];
        }
        interface NumericsData extends Models.DataExport.IMonitorData {
            timestamp: Date;
            values: { [key: string]: Models.DataExport.NumericsValue };
        }
        interface NumericsValue {
            timestamp: Date;
            value: number;
            unit: string;
            state: Enums.MeasurementState;
        }
        interface Observation {
            timestamp: Date;
            parameterName: string;
            value: string;
            unit: string;
        }
        interface UsbDriveInfo {
            path: string;
            name: string;
        }
        interface WaveData extends Models.DataExport.IMonitorData {
            measurementType: string;
            timestampFirstDataPoint: Date;
            sampleRate: number;
            values: System.Single[];
        }
    }
}
export namespace System {
    interface IDisposable {
        
    }
    interface Byte extends System.ValueType, System.IConvertible, System.Numerics.IUnsignedNumber<System.Byte>, System.IUtfChar<System.Byte>, System.IBinaryIntegerParseAndFormatInfo<System.Byte> {
        
    }
    interface Exception extends System.Runtime.Serialization.ISerializable {
        targetSite: System.Reflection.MethodBase;
        message: string;
        data: { [key: string]: any };
        innerException: System.Exception;
        helpLink: string;
        source: string;
        hResult: number;
        stackTrace: string;
    }
    interface Single extends System.ValueType, System.IConvertible, System.IBinaryFloatParseAndFormatInfo<System.Single> {
        
    }
    interface IEquatable<T> {
        
    }
    interface IAsyncDisposable {
        
    }
    interface ValueType {
        
    }
    interface IConvertible {
        
    }
    interface IUtfChar<TSelf> extends System.Numerics.IBinaryInteger<TSelf> {
        
    }
    interface IBinaryIntegerParseAndFormatInfo<TSelf> extends System.Numerics.IBinaryInteger<TSelf>, System.Numerics.IMinMaxValue<TSelf> {
        isSigned: boolean;
        maxDigitCount: number;
        maxHexDigitCount: number;
        maxValueDiv10: TSelf;
        overflowMessage: string;
    }
    interface IBinaryFloatParseAndFormatInfo<TSelf> extends System.Numerics.IBinaryFloatingPointIeee754<TSelf>, System.Numerics.IMinMaxValue<TSelf> {
        numberBufferLength: number;
        zeroBits: number;
        infinityBits: number;
        normalMantissaMask: number;
        denormalMantissaMask: number;
        minBinaryExponent: number;
        maxBinaryExponent: number;
        minDecimalExponent: number;
        maxDecimalExponent: number;
        exponentBias: number;
        exponentBits: number;
        overflowDecimalExponent: number;
        infinityExponent: number;
        normalMantissaBits: number;
        denormalMantissaBits: number;
        minFastFloatDecimalExponent: number;
        maxFastFloatDecimalExponent: number;
        minExponentRoundToEven: number;
        maxExponentRoundToEven: number;
        maxExponentFastPath: number;
        maxMantissaFastPath: number;
    }
    interface RuntimeMethodHandle extends System.ValueType, System.IEquatable<System.RuntimeMethodHandle>, System.Runtime.Serialization.ISerializable {
        value: System.IntPtr;
    }
    interface ISpanFormattable extends System.IFormattable {
        
    }
    interface ISpanParsable<TSelf> extends System.IParsable<TSelf> {
        
    }
    interface IUtf8SpanFormattable {
        
    }
    interface IUtf8SpanParsable<TSelf> {
        
    }
    interface IntPtr extends System.ValueType, System.Runtime.Serialization.ISerializable, System.Numerics.IBinaryInteger<System.IntPtr>, System.Numerics.IMinMaxValue<System.IntPtr>, System.Numerics.ISignedNumber<System.IntPtr> {
        size: number;
    }
    interface Type extends System.Reflection.MemberInfo, System.Reflection.IReflect {
        isInterface: boolean;
        namespace: string;
        assemblyQualifiedName: string;
        fullName: string;
        assembly: System.Reflection.Assembly;
        isNested: boolean;
        declaringMethod: System.Reflection.MethodBase;
        isTypeDefinition: boolean;
        isArray: boolean;
        isByRef: boolean;
        isPointer: boolean;
        isConstructedGenericType: boolean;
        isGenericParameter: boolean;
        isGenericTypeParameter: boolean;
        isGenericMethodParameter: boolean;
        isGenericType: boolean;
        isGenericTypeDefinition: boolean;
        isSZArray: boolean;
        isVariableBoundArray: boolean;
        isByRefLike: boolean;
        isFunctionPointer: boolean;
        isUnmanagedFunctionPointer: boolean;
        hasElementType: boolean;
        genericTypeArguments: System.Type[];
        genericParameterPosition: number;
        genericParameterAttributes: Enums.GenericParameterAttributes;
        attributes: Enums.TypeAttributes;
        isAbstract: boolean;
        isImport: boolean;
        isSealed: boolean;
        isSpecialName: boolean;
        isClass: boolean;
        isNestedAssembly: boolean;
        isNestedFamANDAssem: boolean;
        isNestedFamily: boolean;
        isNestedFamORAssem: boolean;
        isNestedPrivate: boolean;
        isNestedPublic: boolean;
        isNotPublic: boolean;
        isPublic: boolean;
        isAutoLayout: boolean;
        isExplicitLayout: boolean;
        isLayoutSequential: boolean;
        isAnsiClass: boolean;
        isAutoClass: boolean;
        isUnicodeClass: boolean;
        isCOMObject: boolean;
        isContextful: boolean;
        isEnum: boolean;
        isMarshalByRef: boolean;
        isPrimitive: boolean;
        isValueType: boolean;
        isSignatureType: boolean;
        isSecurityCritical: boolean;
        isSecuritySafeCritical: boolean;
        isSecurityTransparent: boolean;
        structLayoutAttribute: System.Runtime.InteropServices.StructLayoutAttribute;
        typeInitializer: System.Reflection.ConstructorInfo;
        typeHandle: System.RuntimeTypeHandle;
        guid: string;
        baseType: System.Type;
        defaultBinder: System.Reflection.Binder;
        isSerializable: boolean;
        containsGenericParameters: boolean;
        isVisible: boolean;
    }
    interface IFormattable {
        
    }
    interface IParsable<TSelf> {
        
    }
    interface RuntimeTypeHandle extends System.ValueType, System.IEquatable<System.RuntimeTypeHandle>, System.Runtime.Serialization.ISerializable {
        value: System.IntPtr;
    }
    interface ModuleHandle extends System.ValueType, System.IEquatable<System.ModuleHandle> {
        mDStreamVersion: number;
    }
    interface IComparable {
        
    }
    interface IComparable<T> {
        
    }
    interface Attribute {
        typeId: any;
    }
    interface RuntimeFieldHandle extends System.ValueType, System.IEquatable<System.RuntimeFieldHandle>, System.Runtime.Serialization.ISerializable {
        value: System.IntPtr;
    }
    export namespace Numerics {
        interface IUnsignedNumber<TSelf> extends System.Numerics.INumberBase<TSelf> {
            
        }
        interface INumberBase<TSelf> extends System.Numerics.IAdditionOperators<TSelf,TSelf,TSelf>, System.Numerics.IAdditiveIdentity<TSelf,TSelf>, System.Numerics.IDecrementOperators<TSelf>, System.Numerics.IDivisionOperators<TSelf,TSelf,TSelf>, System.IEquatable<TSelf>, System.Numerics.IEqualityOperators<TSelf,TSelf,boolean>, System.Numerics.IIncrementOperators<TSelf>, System.Numerics.IMultiplicativeIdentity<TSelf,TSelf>, System.Numerics.IMultiplyOperators<TSelf,TSelf,TSelf>, System.ISpanFormattable, System.ISpanParsable<TSelf>, System.Numerics.ISubtractionOperators<TSelf,TSelf,TSelf>, System.Numerics.IUnaryPlusOperators<TSelf,TSelf>, System.Numerics.IUnaryNegationOperators<TSelf,TSelf>, System.IUtf8SpanFormattable, System.IUtf8SpanParsable<TSelf> {
            one: TSelf;
            radix: number;
            zero: TSelf;
        }
        interface IBinaryInteger<TSelf> extends System.Numerics.IBinaryNumber<TSelf>, System.Numerics.IShiftOperators<TSelf,number,TSelf> {
            
        }
        interface IMinMaxValue<TSelf> {
            minValue: TSelf;
            maxValue: TSelf;
        }
        interface IBinaryFloatingPointIeee754<TSelf> extends System.Numerics.IBinaryNumber<TSelf>, System.Numerics.IFloatingPointIeee754<TSelf> {
            
        }
        interface IAdditionOperators<TSelf,TOther,TResult> {
            
        }
        interface IAdditiveIdentity<TSelf,TResult> {
            additiveIdentity: TResult;
        }
        interface IDecrementOperators<TSelf> {
            
        }
        interface IDivisionOperators<TSelf,TOther,TResult> {
            
        }
        interface IEqualityOperators<TSelf,TOther,TResult> {
            
        }
        interface IIncrementOperators<TSelf> {
            
        }
        interface IMultiplicativeIdentity<TSelf,TResult> {
            multiplicativeIdentity: TResult;
        }
        interface IMultiplyOperators<TSelf,TOther,TResult> {
            
        }
        interface ISubtractionOperators<TSelf,TOther,TResult> {
            
        }
        interface IUnaryPlusOperators<TSelf,TResult> {
            
        }
        interface IUnaryNegationOperators<TSelf,TResult> {
            
        }
        interface IBinaryNumber<TSelf> extends System.Numerics.IBitwiseOperators<TSelf,TSelf,TSelf>, System.Numerics.INumber<TSelf> {
            allBitsSet: TSelf;
        }
        interface IShiftOperators<TSelf,TOther,TResult> {
            
        }
        interface IFloatingPointIeee754<TSelf> extends System.Numerics.IExponentialFunctions<TSelf>, System.Numerics.IFloatingPoint<TSelf>, System.Numerics.IHyperbolicFunctions<TSelf>, System.Numerics.ILogarithmicFunctions<TSelf>, System.Numerics.IPowerFunctions<TSelf>, System.Numerics.IRootFunctions<TSelf>, System.Numerics.ITrigonometricFunctions<TSelf> {
            epsilon: TSelf;
            naN: TSelf;
            negativeInfinity: TSelf;
            negativeZero: TSelf;
            positiveInfinity: TSelf;
        }
        interface IBitwiseOperators<TSelf,TOther,TResult> {
            
        }
        interface INumber<TSelf> extends System.IComparable, System.IComparable<TSelf>, System.Numerics.IComparisonOperators<TSelf,TSelf,boolean>, System.Numerics.IModulusOperators<TSelf,TSelf,TSelf>, System.Numerics.INumberBase<TSelf> {
            
        }
        interface ISignedNumber<TSelf> extends System.Numerics.INumberBase<TSelf> {
            negativeOne: TSelf;
        }
        interface IExponentialFunctions<TSelf> extends System.Numerics.IFloatingPointConstants<TSelf> {
            
        }
        interface IFloatingPoint<TSelf> extends System.Numerics.IFloatingPointConstants<TSelf>, System.Numerics.INumber<TSelf>, System.Numerics.ISignedNumber<TSelf> {
            
        }
        interface IHyperbolicFunctions<TSelf> extends System.Numerics.IFloatingPointConstants<TSelf> {
            
        }
        interface ILogarithmicFunctions<TSelf> extends System.Numerics.IFloatingPointConstants<TSelf> {
            
        }
        interface IPowerFunctions<TSelf> extends System.Numerics.INumberBase<TSelf> {
            
        }
        interface IRootFunctions<TSelf> extends System.Numerics.IFloatingPointConstants<TSelf> {
            
        }
        interface ITrigonometricFunctions<TSelf> extends System.Numerics.IFloatingPointConstants<TSelf> {
            
        }
        interface IComparisonOperators<TSelf,TOther,TResult> extends System.Numerics.IEqualityOperators<TSelf,TOther,TResult> {
            
        }
        interface IModulusOperators<TSelf,TOther,TResult> {
            
        }
        interface IFloatingPointConstants<TSelf> extends System.Numerics.INumberBase<TSelf> {
            e: TSelf;
            pi: TSelf;
            tau: TSelf;
        }
    }
    export namespace Reflection {
        interface MethodBase extends System.Reflection.MemberInfo {
            attributes: Enums.MethodAttributes;
            methodImplementationFlags: Enums.MethodImplAttributes;
            callingConvention: Enums.CallingConventions;
            isAbstract: boolean;
            isConstructor: boolean;
            isFinal: boolean;
            isHideBySig: boolean;
            isSpecialName: boolean;
            isStatic: boolean;
            isVirtual: boolean;
            isAssembly: boolean;
            isFamily: boolean;
            isFamilyAndAssembly: boolean;
            isFamilyOrAssembly: boolean;
            isPrivate: boolean;
            isPublic: boolean;
            isConstructedGenericMethod: boolean;
            isGenericMethod: boolean;
            isGenericMethodDefinition: boolean;
            containsGenericParameters: boolean;
            methodHandle: System.RuntimeMethodHandle;
            isSecurityCritical: boolean;
            isSecuritySafeCritical: boolean;
            isSecurityTransparent: boolean;
        }
        interface MemberInfo extends System.Reflection.ICustomAttributeProvider {
            memberType: Enums.MemberTypes;
            name: string;
            declaringType: System.Type;
            reflectedType: System.Type;
            module: System.Reflection.Module;
            customAttributes: System.Reflection.CustomAttributeData[];
            isCollectible: boolean;
            metadataToken: number;
        }
        interface Module extends System.Reflection.ICustomAttributeProvider, System.Runtime.Serialization.ISerializable {
            assembly: System.Reflection.Assembly;
            fullyQualifiedName: string;
            name: string;
            mDStreamVersion: number;
            moduleVersionId: string;
            scopeName: string;
            moduleHandle: System.ModuleHandle;
            customAttributes: System.Reflection.CustomAttributeData[];
            metadataToken: number;
        }
        interface CustomAttributeData {
            attributeType: System.Type;
            constructor: System.Reflection.ConstructorInfo;
            constructorArguments: System.Reflection.CustomAttributeTypedArgument[];
            namedArguments: System.Reflection.CustomAttributeNamedArgument[];
        }
        interface ICustomAttributeProvider {
            
        }
        interface Assembly extends System.Reflection.ICustomAttributeProvider, System.Runtime.Serialization.ISerializable {
            definedTypes: System.Reflection.TypeInfo[];
            exportedTypes: System.Type[];
            codeBase: string;
            entryPoint: System.Reflection.MethodInfo;
            fullName: string;
            imageRuntimeVersion: string;
            isDynamic: boolean;
            location: string;
            reflectionOnly: boolean;
            isCollectible: boolean;
            isFullyTrusted: boolean;
            customAttributes: System.Reflection.CustomAttributeData[];
            escapedCodeBase: string;
            manifestModule: System.Reflection.Module;
            modules: System.Reflection.Module[];
            globalAssemblyCache: boolean;
            hostContext: number;
            securityRuleSet: Enums.SecurityRuleSet;
        }
        interface ConstructorInfo extends System.Reflection.MethodBase {
            
        }
        interface Binder {
            
        }
        interface IReflect {
            underlyingSystemType: System.Type;
        }
        interface CustomAttributeTypedArgument extends System.ValueType, System.IEquatable<System.Reflection.CustomAttributeTypedArgument> {
            argumentType: System.Type;
            value: any;
        }
        interface CustomAttributeNamedArgument extends System.ValueType, System.IEquatable<System.Reflection.CustomAttributeNamedArgument> {
            memberInfo: System.Reflection.MemberInfo;
            typedValue: System.Reflection.CustomAttributeTypedArgument;
            memberName: string;
            isField: boolean;
        }
        interface TypeInfo extends System.Type, System.Reflection.IReflectableType {
            genericTypeParameters: System.Type[];
            declaredConstructors: System.Reflection.ConstructorInfo[];
            declaredEvents: System.Reflection.EventInfo[];
            declaredFields: System.Reflection.FieldInfo[];
            declaredMembers: System.Reflection.MemberInfo[];
            declaredMethods: System.Reflection.MethodInfo[];
            declaredNestedTypes: System.Reflection.TypeInfo[];
            declaredProperties: System.Reflection.PropertyInfo[];
            implementedInterfaces: System.Type[];
        }
        interface MethodInfo extends System.Reflection.MethodBase {
            returnParameter: System.Reflection.ParameterInfo;
            returnType: System.Type;
            returnTypeCustomAttributes: System.Reflection.ICustomAttributeProvider;
        }
        interface EventInfo extends System.Reflection.MemberInfo {
            attributes: Enums.EventAttributes;
            isSpecialName: boolean;
            addMethod: System.Reflection.MethodInfo;
            removeMethod: System.Reflection.MethodInfo;
            raiseMethod: System.Reflection.MethodInfo;
            isMulticast: boolean;
            eventHandlerType: System.Type;
        }
        interface FieldInfo extends System.Reflection.MemberInfo {
            attributes: Enums.FieldAttributes;
            fieldType: System.Type;
            isInitOnly: boolean;
            isLiteral: boolean;
            isNotSerialized: boolean;
            isPinvokeImpl: boolean;
            isSpecialName: boolean;
            isStatic: boolean;
            isAssembly: boolean;
            isFamily: boolean;
            isFamilyAndAssembly: boolean;
            isFamilyOrAssembly: boolean;
            isPrivate: boolean;
            isPublic: boolean;
            isSecurityCritical: boolean;
            isSecuritySafeCritical: boolean;
            isSecurityTransparent: boolean;
            fieldHandle: System.RuntimeFieldHandle;
        }
        interface PropertyInfo extends System.Reflection.MemberInfo {
            propertyType: System.Type;
            attributes: Enums.PropertyAttributes;
            isSpecialName: boolean;
            canRead: boolean;
            canWrite: boolean;
            getMethod: System.Reflection.MethodInfo;
            setMethod: System.Reflection.MethodInfo;
        }
        interface IReflectableType {
            
        }
        interface ParameterInfo extends System.Reflection.ICustomAttributeProvider, System.Runtime.Serialization.IObjectReference {
            attributes: Enums.ParameterAttributes;
            member: System.Reflection.MemberInfo;
            name: string;
            parameterType: System.Type;
            position: number;
            isIn: boolean;
            isLcid: boolean;
            isOptional: boolean;
            isOut: boolean;
            isRetval: boolean;
            defaultValue: any;
            rawDefaultValue: any;
            hasDefaultValue: boolean;
            customAttributes: System.Reflection.CustomAttributeData[];
            metadataToken: number;
        }
    }
    export namespace Runtime {
        export namespace Serialization {
            interface ISerializable {
                
            }
            interface IObjectReference {
                
            }
        }
        export namespace InteropServices {
            interface StructLayoutAttribute extends System.Attribute {
                value: Enums.LayoutKind;
            }
        }
    }
}
