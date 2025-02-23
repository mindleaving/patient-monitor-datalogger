import * as Enums from './enums';

export namespace Models {
    interface GEDashPatientMonitorSettings extends Models.PatientMonitorSettings {
        serialPortName: string;
        serialPortBaudRate: number;
    }
    interface IPatientMonitorInfo {
        type: Enums.PatientMonitorType;
    }
    interface LogSession extends System.IDisposable, System.IAsyncDisposable {
        id: string;
        settings: Models.LogSessionSettings;
        patientInfo: Models.PatientInfo;
        latestMeasurements: { [measurementType: string]: Models.DataExport.NumericsValue };
        shouldBeRunning: boolean;
        status: Models.LogStatus;
    }
    interface LogSessionSettings {
        monitorSettings: Models.PatientMonitorSettings;
        selectedNumericsTypes: Enums.MeasurementType[];
        selectedWaveTypes: Enums.MeasurementType[];
        csvSeparator: string;
    }
    interface LogStatus {
        isRunning: boolean;
        monitor: Models.IPatientMonitorInfo;
        startTime?: Date | null;
        recordedNumerics: Enums.MeasurementType[];
        recordedWaves: Enums.MeasurementType[];
    }
    interface MonitorDataWriterSettings {
        outputDirectory: string;
    }
    interface PatientInfo {
        patientId: string;
        encounterId: string;
        firstName: string;
        lastName: string;
        sex?: Enums.Sex | null;
        dateOfBirth?: Date | null;
        comment: string;
    }
    interface PatientMonitorSettings {
        type: Enums.PatientMonitorType;
    }
    interface PhilipsIntellivuePatientMonitorInfo extends Models.IPatientMonitorInfo {
        name: string;
    }
    interface PhilipsIntellivuePatientMonitorSettings extends Models.PatientMonitorSettings {
        serialPortName: string;
        serialPortBaudRate: number;
    }
    export namespace RequestBodies {
        interface CopyDataToUsbDriveRequest {
            usbDrivePath: string;
        }
    }
    export namespace Converters {
        interface PatientMonitorSettingsJsonConverter extends Newtonsoft.Json.JsonConverter<Models.PatientMonitorSettings> {
            
        }
    }
    export namespace DataExport {
        interface IMonitorData {
            type: Enums.MonitorDataType;
            logSessionId: string;
        }
        interface NumericsData extends Models.DataExport.IMonitorData {
            timestamp: Date;
            values: { [measurementType: string]: Models.DataExport.NumericsValue };
        }
        interface NumericsValue {
            timestamp: Date;
            value: number;
            unit: string;
        }
        interface UsbDriveInfo {
            path: string;
            name: string;
        }
        interface WaveData extends Models.DataExport.IMonitorData {
            measurementType: Enums.MeasurementType;
            timestampFirstDataPoint: Date;
            sampleRate: number;
            values: System.Single[];
        }
    }
}
export namespace System {
    interface IDisposable {
        
    }
    interface IAsyncDisposable {
        
    }
    interface Single extends System.ValueType, System.IConvertible, System.IBinaryFloatParseAndFormatInfo<System.Single> {
        
    }
    interface ValueType {
        
    }
    interface IConvertible {
        
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
    interface IComparable {
        
    }
    interface IComparable<T> {
        
    }
    interface IEquatable<T> {
        
    }
    interface ISpanFormattable extends System.IFormattable {
        
    }
    interface ISpanParsable<TSelf> extends System.IParsable<TSelf> {
        
    }
    interface IUtf8SpanFormattable {
        
    }
    interface IUtf8SpanParsable<TSelf> {
        
    }
    interface IFormattable {
        
    }
    interface IParsable<TSelf> {
        
    }
    export namespace Numerics {
        interface IBinaryFloatingPointIeee754<TSelf> extends System.Numerics.IBinaryNumber<TSelf>, System.Numerics.IFloatingPointIeee754<TSelf> {
            
        }
        interface IMinMaxValue<TSelf> {
            minValue: TSelf;
            maxValue: TSelf;
        }
        interface IBinaryNumber<TSelf> extends System.Numerics.IBitwiseOperators<TSelf,TSelf,TSelf>, System.Numerics.INumber<TSelf> {
            allBitsSet: TSelf;
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
        interface INumberBase<TSelf> extends System.Numerics.IAdditionOperators<TSelf,TSelf,TSelf>, System.Numerics.IAdditiveIdentity<TSelf,TSelf>, System.Numerics.IDecrementOperators<TSelf>, System.Numerics.IDivisionOperators<TSelf,TSelf,TSelf>, System.IEquatable<TSelf>, System.Numerics.IEqualityOperators<TSelf,TSelf,boolean>, System.Numerics.IIncrementOperators<TSelf>, System.Numerics.IMultiplicativeIdentity<TSelf,TSelf>, System.Numerics.IMultiplyOperators<TSelf,TSelf,TSelf>, System.ISpanFormattable, System.ISpanParsable<TSelf>, System.Numerics.ISubtractionOperators<TSelf,TSelf,TSelf>, System.Numerics.IUnaryPlusOperators<TSelf,TSelf>, System.Numerics.IUnaryNegationOperators<TSelf,TSelf>, System.IUtf8SpanFormattable, System.IUtf8SpanParsable<TSelf> {
            one: TSelf;
            radix: number;
            zero: TSelf;
        }
        interface IFloatingPointConstants<TSelf> extends System.Numerics.INumberBase<TSelf> {
            e: TSelf;
            pi: TSelf;
            tau: TSelf;
        }
        interface ISignedNumber<TSelf> extends System.Numerics.INumberBase<TSelf> {
            negativeOne: TSelf;
        }
        interface IEqualityOperators<TSelf,TOther,TResult> {
            
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
    }
}
export namespace Newtonsoft {
    export namespace Json {
        interface JsonConverter<T> extends Newtonsoft.Json.JsonConverter {
            
        }
        interface JsonConverter {
            canRead: boolean;
            canWrite: boolean;
        }
    }
}
