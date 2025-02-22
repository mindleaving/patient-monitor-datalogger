export enum PatientMonitorType {
    Unknown = "Unknown",
    PhilipsIntellivue = "PhilipsIntellivue",
    GEDash = "GEDash"
}
export enum MeasurementType {
    Undefined = "Undefined",
    HeartRateEcg = "HeartRateEcg",
    HeartRateSpO2 = "HeartRateSpO2",
    SpO2 = "SpO2",
    SpO2preDuctal = "SpO2preDuctal",
    RespirationRate = "RespirationRate",
    EcgLeadI = "EcgLeadI",
    EcgLeadII = "EcgLeadII",
    EcgLeadIII = "EcgLeadIII",
    EcgLeadavR = "EcgLeadavR",
    EcgLeadavL = "EcgLeadavL",
    EcgLeadavF = "EcgLeadavF",
    EcgLeadV1 = "EcgLeadV1",
    EcgLeadV2 = "EcgLeadV2",
    EcgLeadV3 = "EcgLeadV3",
    EcgLeadV4 = "EcgLeadV4",
    EcgLeadV5 = "EcgLeadV5",
    EcgLeadV6 = "EcgLeadV6",
    EcgLeadV7 = "EcgLeadV7",
    EcgLeadV8 = "EcgLeadV8",
    EcgLeadV9 = "EcgLeadV9"
}
export enum GenericParameterAttributes {
    None = "None",
    Covariant = "Covariant",
    Contravariant = "Contravariant",
    VarianceMask = "VarianceMask",
    ReferenceTypeConstraint = "ReferenceTypeConstraint",
    NotNullableValueTypeConstraint = "NotNullableValueTypeConstraint",
    DefaultConstructorConstraint = "DefaultConstructorConstraint",
    SpecialConstraintMask = "SpecialConstraintMask"
}
export enum TypeAttributes {
    NotPublic = "NotPublic",
    AutoLayout = "AutoLayout",
    AnsiClass = "AnsiClass",
    Class = "Class",
    Public = "Public",
    NestedPublic = "NestedPublic",
    NestedPrivate = "NestedPrivate",
    NestedFamily = "NestedFamily",
    NestedAssembly = "NestedAssembly",
    NestedFamANDAssem = "NestedFamANDAssem",
    VisibilityMask = "VisibilityMask",
    NestedFamORAssem = "NestedFamORAssem",
    SequentialLayout = "SequentialLayout",
    ExplicitLayout = "ExplicitLayout",
    LayoutMask = "LayoutMask",
    Interface = "Interface",
    ClassSemanticsMask = "ClassSemanticsMask",
    Abstract = "Abstract",
    Sealed = "Sealed",
    SpecialName = "SpecialName",
    RTSpecialName = "RTSpecialName",
    Import = "Import",
    Serializable = "Serializable",
    WindowsRuntime = "WindowsRuntime",
    UnicodeClass = "UnicodeClass",
    AutoClass = "AutoClass",
    StringFormatMask = "StringFormatMask",
    CustomFormatClass = "CustomFormatClass",
    HasSecurity = "HasSecurity",
    ReservedMask = "ReservedMask",
    BeforeFieldInit = "BeforeFieldInit",
    CustomFormatMask = "CustomFormatMask"
}
export enum SecurityRuleSet {
    None = "None",
    Level1 = "Level1",
    Level2 = "Level2"
}
export enum MethodAttributes {
    PrivateScope = "PrivateScope",
    ReuseSlot = "ReuseSlot",
    Private = "Private",
    FamANDAssem = "FamANDAssem",
    Assembly = "Assembly",
    Family = "Family",
    FamORAssem = "FamORAssem",
    Public = "Public",
    MemberAccessMask = "MemberAccessMask",
    UnmanagedExport = "UnmanagedExport",
    Static = "Static",
    Final = "Final",
    Virtual = "Virtual",
    HideBySig = "HideBySig",
    NewSlot = "NewSlot",
    VtableLayoutMask = "VtableLayoutMask",
    CheckAccessOnOverride = "CheckAccessOnOverride",
    Abstract = "Abstract",
    SpecialName = "SpecialName",
    RTSpecialName = "RTSpecialName",
    PinvokeImpl = "PinvokeImpl",
    HasSecurity = "HasSecurity",
    RequireSecObject = "RequireSecObject",
    ReservedMask = "ReservedMask"
}
export enum MethodImplAttributes {
    IL = "IL",
    Managed = "Managed",
    Native = "Native",
    OPTIL = "OPTIL",
    CodeTypeMask = "CodeTypeMask",
    Runtime = "Runtime",
    ManagedMask = "ManagedMask",
    Unmanaged = "Unmanaged",
    NoInlining = "NoInlining",
    ForwardRef = "ForwardRef",
    Synchronized = "Synchronized",
    NoOptimization = "NoOptimization",
    PreserveSig = "PreserveSig",
    AggressiveInlining = "AggressiveInlining",
    AggressiveOptimization = "AggressiveOptimization",
    InternalCall = "InternalCall",
    MaxMethodImplVal = "MaxMethodImplVal"
}
export enum CallingConventions {
    Standard = "Standard",
    VarArgs = "VarArgs",
    Any = "Any",
    HasThis = "HasThis",
    ExplicitThis = "ExplicitThis"
}
export enum LayoutKind {
    Sequential = "Sequential",
    Explicit = "Explicit",
    Auto = "Auto"
}
export enum MemberTypes {
    Constructor = "Constructor",
    Event = "Event",
    Field = "Field",
    Method = "Method",
    Property = "Property",
    TypeInfo = "TypeInfo",
    Custom = "Custom",
    NestedType = "NestedType",
    All = "All"
}
export enum EventAttributes {
    None = "None",
    SpecialName = "SpecialName",
    RTSpecialName = "RTSpecialName",
    ReservedMask = "ReservedMask"
}
export enum FieldAttributes {
    PrivateScope = "PrivateScope",
    Private = "Private",
    FamANDAssem = "FamANDAssem",
    Assembly = "Assembly",
    Family = "Family",
    FamORAssem = "FamORAssem",
    Public = "Public",
    FieldAccessMask = "FieldAccessMask",
    Static = "Static",
    InitOnly = "InitOnly",
    Literal = "Literal",
    NotSerialized = "NotSerialized",
    HasFieldRVA = "HasFieldRVA",
    SpecialName = "SpecialName",
    RTSpecialName = "RTSpecialName",
    HasFieldMarshal = "HasFieldMarshal",
    PinvokeImpl = "PinvokeImpl",
    HasDefault = "HasDefault",
    ReservedMask = "ReservedMask"
}
export enum PropertyAttributes {
    None = "None",
    SpecialName = "SpecialName",
    RTSpecialName = "RTSpecialName",
    HasDefault = "HasDefault",
    Reserved2 = "Reserved2",
    Reserved3 = "Reserved3",
    Reserved4 = "Reserved4",
    ReservedMask = "ReservedMask"
}
export enum ParameterAttributes {
    None = "None",
    In = "In",
    Out = "Out",
    Lcid = "Lcid",
    Retval = "Retval",
    Optional = "Optional",
    HasDefault = "HasDefault",
    HasFieldMarshal = "HasFieldMarshal",
    Reserved3 = "Reserved3",
    Reserved4 = "Reserved4",
    ReservedMask = "ReservedMask"
}
