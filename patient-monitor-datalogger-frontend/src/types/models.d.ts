import * as Enums from './enums';

export namespace Models {
    interface GEDashPatientMonitorSettings extends Models.IPatientMonitorSettings {
        serialPortName: string;
        serialPortBaudRate: number;
    }
    interface IPatientMonitorInfo {
        type: Enums.PatientMonitorType;
    }
    interface IPatientMonitorSettings {
        type: Enums.PatientMonitorType;
    }
    interface LogSession extends System.IDisposable, System.IAsyncDisposable {
        id: string;
        settings: Models.LogSessionSettings;
        status: Models.LogStatus;
    }
    interface LogSessionSettings {
        monitorSettings: Models.IPatientMonitorSettings;
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
    interface PhilipsIntellivuePatientMonitorInfo extends Models.IPatientMonitorInfo {
        name: string;
    }
    interface PhilipsIntellivuePatientMonitorSettings extends Models.IPatientMonitorSettings {
        serialPortName: string;
        serialPortBaudRate: number;
    }
    export namespace RequestBodies {
        interface CopyDataToUsbDriveRequest {
            usbDrivePath: string;
        }
    }
    export namespace Converters {
        interface PatientMonitorSettingsJsonConverter extends System.Text.Json.Serialization.JsonConverter<Models.IPatientMonitorSettings> {
            
        }
    }
}
export namespace System {
    interface IDisposable {
        
    }
    interface IAsyncDisposable {
        
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
    interface RuntimeTypeHandle extends System.ValueType, System.IEquatable<System.RuntimeTypeHandle>, System.Runtime.Serialization.ISerializable {
        value: System.IntPtr;
    }
    interface RuntimeMethodHandle extends System.ValueType, System.IEquatable<System.RuntimeMethodHandle>, System.Runtime.Serialization.ISerializable {
        value: System.IntPtr;
    }
    interface Attribute {
        typeId: any;
    }
    interface IntPtr extends System.ValueType, System.Runtime.Serialization.ISerializable, System.Numerics.IBinaryInteger<System.IntPtr>, System.Numerics.IMinMaxValue<System.IntPtr>, System.Numerics.ISignedNumber<System.IntPtr> {
        size: number;
    }
    interface ValueType {
        
    }
    interface IEquatable<T> {
        
    }
    interface ModuleHandle extends System.ValueType, System.IEquatable<System.ModuleHandle> {
        mDStreamVersion: number;
    }
    interface RuntimeFieldHandle extends System.ValueType, System.IEquatable<System.RuntimeFieldHandle>, System.Runtime.Serialization.ISerializable {
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
    interface IComparable {
        
    }
    interface IComparable<T> {
        
    }
    interface IFormattable {
        
    }
    interface IParsable<TSelf> {
        
    }
    export namespace Text {
        export namespace Json {
            export namespace Serialization {
                interface JsonConverter<T> extends System.Text.Json.Serialization.JsonConverter {
                    handleNull: boolean;
                }
                interface JsonConverter {
                    type: System.Type;
                }
            }
        }
    }
    export namespace Reflection {
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
        interface ConstructorInfo extends System.Reflection.MethodBase {
            
        }
        interface Binder {
            
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
        interface IReflect {
            underlyingSystemType: System.Type;
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
        interface CustomAttributeData {
            attributeType: System.Type;
            constructor: System.Reflection.ConstructorInfo;
            constructorArguments: System.Reflection.CustomAttributeTypedArgument[];
            namedArguments: System.Reflection.CustomAttributeNamedArgument[];
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
        interface ICustomAttributeProvider {
            
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
    }
    export namespace Runtime {
        export namespace InteropServices {
            interface StructLayoutAttribute extends System.Attribute {
                value: Enums.LayoutKind;
            }
        }
        export namespace Serialization {
            interface ISerializable {
                
            }
            interface IObjectReference {
                
            }
        }
    }
    export namespace Numerics {
        interface IBinaryInteger<TSelf> extends System.Numerics.IBinaryNumber<TSelf>, System.Numerics.IShiftOperators<TSelf,number,TSelf> {
            
        }
        interface IMinMaxValue<TSelf> {
            minValue: TSelf;
            maxValue: TSelf;
        }
        interface ISignedNumber<TSelf> extends System.Numerics.INumberBase<TSelf> {
            negativeOne: TSelf;
        }
        interface IBinaryNumber<TSelf> extends System.Numerics.IBitwiseOperators<TSelf,TSelf,TSelf>, System.Numerics.INumber<TSelf> {
            allBitsSet: TSelf;
        }
        interface IShiftOperators<TSelf,TOther,TResult> {
            
        }
        interface INumberBase<TSelf> extends System.Numerics.IAdditionOperators<TSelf,TSelf,TSelf>, System.Numerics.IAdditiveIdentity<TSelf,TSelf>, System.Numerics.IDecrementOperators<TSelf>, System.Numerics.IDivisionOperators<TSelf,TSelf,TSelf>, System.IEquatable<TSelf>, System.Numerics.IEqualityOperators<TSelf,TSelf,boolean>, System.Numerics.IIncrementOperators<TSelf>, System.Numerics.IMultiplicativeIdentity<TSelf,TSelf>, System.Numerics.IMultiplyOperators<TSelf,TSelf,TSelf>, System.ISpanFormattable, System.ISpanParsable<TSelf>, System.Numerics.ISubtractionOperators<TSelf,TSelf,TSelf>, System.Numerics.IUnaryPlusOperators<TSelf,TSelf>, System.Numerics.IUnaryNegationOperators<TSelf,TSelf>, System.IUtf8SpanFormattable, System.IUtf8SpanParsable<TSelf> {
            one: TSelf;
            radix: number;
            zero: TSelf;
        }
        interface IBitwiseOperators<TSelf,TOther,TResult> {
            
        }
        interface INumber<TSelf> extends System.IComparable, System.IComparable<TSelf>, System.Numerics.IComparisonOperators<TSelf,TSelf,boolean>, System.Numerics.IModulusOperators<TSelf,TSelf,TSelf>, System.Numerics.INumberBase<TSelf> {
            
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
        interface IComparisonOperators<TSelf,TOther,TResult> extends System.Numerics.IEqualityOperators<TSelf,TOther,TResult> {
            
        }
        interface IModulusOperators<TSelf,TOther,TResult> {
            
        }
    }
}
