namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public static class PollAttributeGroups
{
    public static class Numerics
    {
        public const OIDType VmoStaticContext = OIDType.NOM_ATTR_GRP_VMO_STATIC;
        public const OIDType VmoDynamicContext = OIDType.NOM_ATTR_GRP_VMO_DYN;
        public const OIDType MetricObservedValue = OIDType.NOM_ATTR_GRP_METRIC_VAL_OBS;
    }
    public static class Waves
    {
        public const OIDType VmoStaticContext = OIDType.NOM_ATTR_GRP_VMO_STATIC;
        public const OIDType VmoDynamicContext = OIDType.NOM_ATTR_GRP_VMO_DYN;
        public const OIDType MetricObservedValue = OIDType.NOM_ATTR_GRP_METRIC_VAL_OBS;
    }
    public static class Enumerations
    {
        public const OIDType VmoStaticContext = OIDType.NOM_ATTR_GRP_VMO_STATIC;
        public const OIDType VmoDynamicContext = OIDType.NOM_ATTR_GRP_VMO_DYN;
        public const OIDType MetricObservedValue = OIDType.NOM_ATTR_GRP_METRIC_VAL_OBS;
    }
    public static class System
    {
        public const OIDType SystemIdentification = OIDType.NOM_ATTR_GRP_SYS_ID;
        public const OIDType SystemApplication = OIDType.NOM_ATTR_GRP_SYS_APPL;
        public const OIDType SystemProduction = OIDType.NOM_ATTR_GRP_SYS_PROD;
    }
    public static class Alerts
    {
        public const OIDType VmoStaticContext = OIDType.NOM_ATTR_GRP_VMO_STATIC;
        public const OIDType AlertMonitor = OIDType.NOM_ATTR_GRP_AL_MON;
    }
    public static class PatientDemographics
    {
        public const OIDType All = OIDType.NOM_ATTR_GRP_PT_DEMOG;
    }
}