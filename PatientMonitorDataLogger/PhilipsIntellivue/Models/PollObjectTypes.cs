namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public static class PollObjectTypes
{
    public static readonly NomenclatureReference Numerics = new(NomenclaturePartition.NOM_PART_OBJ, (ushort)ObjectClass.NOM_MOC_VMO_METRIC_NU);
    public static readonly NomenclatureReference Waves = new(NomenclaturePartition.NOM_PART_OBJ, (ushort)ObjectClass.NOM_MOC_VMO_METRIC_SA_RT);
    public static readonly NomenclatureReference Alerts = new(NomenclaturePartition.NOM_PART_OBJ, (ushort)ObjectClass.NOM_MOC_VMO_AL_MON);
    public static readonly NomenclatureReference PatientDemographics = new(NomenclaturePartition.NOM_PART_OBJ, (ushort)ObjectClass.NOM_MOC_PT_DEMOG);
    public static readonly NomenclatureReference MDS = new(NomenclaturePartition.NOM_PART_OBJ, (ushort)ObjectClass.NOM_MOC_VMS_MDS);
}