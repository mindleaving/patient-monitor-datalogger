﻿namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class UnitCodesMap
{
    public static readonly Dictionary<ushort, string> CodeToUnitMap = new()
    {
        { (ushort)UnitCodes.NOM_DIM_NOS, "" },
        { (ushort)UnitCodes.NOM_DIM_DIV, "/" },
        { (ushort)UnitCodes.NOM_DIM_DIMLESS, "-" },
        { (ushort)UnitCodes.NOM_DIM_PERCENT, "%" },
        { (ushort)UnitCodes.NOM_DIM_PARTS_PER_THOUSAND, "ppth" },
        { (ushort)UnitCodes.NOM_DIM_PARTS_PER_MILLION, "ppm" },
        { (ushort)UnitCodes.NOM_DIM_X_MOLE_PER_MOLE, "mol/mol" },
        { (ushort)UnitCodes.NOM_DIM_PARTS_PER_BILLION, "ppb" },
        { (ushort)UnitCodes.NOM_DIM_PARTS_PER_TRILLION, "ppt" },
        { (ushort)UnitCodes.NOM_DIM_PH, "pH" },
        { (ushort)UnitCodes.NOM_DIM_DROP, "drop" },
        { (ushort)UnitCodes.NOM_DIM_RBC, "rbc" },
        { (ushort)UnitCodes.NOM_DIM_BEAT, "beat" },
        { (ushort)UnitCodes.NOM_DIM_BREATH, "breath" },
        { (ushort)UnitCodes.NOM_DIM_CELL, "cell" },
        { (ushort)UnitCodes.NOM_DIM_COUGH, "cough" },
        { (ushort)UnitCodes.NOM_DIM_SIGH, "sigh" },
        { (ushort)UnitCodes.NOM_DIM_PCT_PCV, "%PCV" },
        { (ushort)UnitCodes.NOM_DIM_X_M, "m" },
        { (ushort)UnitCodes.NOM_DIM_CENTI_M, "cm" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_M, "mm" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_M, "µm" },
        { (ushort)UnitCodes.NOM_DIM_X_INCH, "in" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_M_SQ, "ml/m2" },
        { (ushort)UnitCodes.NOM_DIM_PER_X_M, "/m" },
        { (ushort)UnitCodes.NOM_DIM_PER_MILLI_M, "/mm" },
        { (ushort)UnitCodes.NOM_DIM_SQ_X_M, "m2" },
        { (ushort)UnitCodes.NOM_DIM_SQ_X_INCH, "in2" },
        { (ushort)UnitCodes.NOM_DIM_CUBIC_X_M, "m3" },
        { (ushort)UnitCodes.NOM_DIM_CUBIC_CENTI_M, "cm3" },
        { (ushort)UnitCodes.NOM_DIM_X_L, "l" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L, "ml" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_BREATH, "ml/breath" },
        { (ushort)UnitCodes.NOM_DIM_PER_CUBIC_CENTI_M, "/cm3" },
        { (ushort)UnitCodes.NOM_DIM_PER_X_L, "/l" },
        { (ushort)UnitCodes.NOM_DIM_PER_NANO_LITER, "1/nl" },
        { (ushort)UnitCodes.NOM_DIM_X_G, "g" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G, "kg" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G, "mg" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G, "µg" },
        { (ushort)UnitCodes.NOM_DIM_NANO_G, "ng" },
        { (ushort)UnitCodes.NOM_DIM_X_LB, "lb" },
        { (ushort)UnitCodes.NOM_DIM_X_OZ, "oz" },
        { (ushort)UnitCodes.NOM_DIM_PER_X_G, "/g" },
        { (ushort)UnitCodes.NOM_DIM_X_G_M, "g-m" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_M, "kg-m" },
        { (ushort)UnitCodes.NOM_DIM_X_G_M_PER_M_SQ, "g-m/m2" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_M_PER_M_SQ, "kg-m/m2" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_M_SQ, "kg-m2" },
        { (ushort)UnitCodes.NOM_DIM_KG_PER_M_SQ, "kg/m2" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_PER_M_CUBE, "kg/m3" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_CM_CUBE, "g/cm3" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_CM_CUBE, "mg/cm3" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G_PER_CM_CUBE, "µg/cm3" },
        { (ushort)UnitCodes.NOM_DIM_NANO_G_PER_CM_CUBE, "ng/cm3" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_L, "g/l" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_DL, "g/dl" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_DL, "mg/dl" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_ML, "g/ml" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_ML, "mg/ml" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G_PER_ML, "µg/ml" },
        { (ushort)UnitCodes.NOM_DIM_NANO_G_PER_ML, "ng/ml" },
        { (ushort)UnitCodes.NOM_DIM_SEC, "sec" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_SEC, "msec" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_SEC, "µsec" },
        { (ushort)UnitCodes.NOM_DIM_MIN, "min" },
        { (ushort)UnitCodes.NOM_DIM_HR, "hrs" },
        { (ushort)UnitCodes.NOM_DIM_DAY, "days" },
        { (ushort)UnitCodes.NOM_DIM_WEEKS, "weeks" },
        { (ushort)UnitCodes.NOM_DIM_MON, "months" },
        { (ushort)UnitCodes.NOM_DIM_YR, "years" },
        { (ushort)UnitCodes.NOM_DIM_TOD, "TOD" },
        { (ushort)UnitCodes.NOM_DIM_DATE, "date" },
        { (ushort)UnitCodes.NOM_DIM_PER_X_SEC, "/sec" },
        { (ushort)UnitCodes.NOM_DIM_HZ, "Hz" },
        { (ushort)UnitCodes.NOM_DIM_PER_MIN, "/min" },
        { (ushort)UnitCodes.NOM_DIM_PER_HR, "/hour" },
        { (ushort)UnitCodes.NOM_DIM_PER_DAY, "/day" },
        { (ushort)UnitCodes.NOM_DIM_PER_WK, "/week" },
        { (ushort)UnitCodes.NOM_DIM_PER_MO, "/month" },
        { (ushort)UnitCodes.NOM_DIM_PER_YR, "/year" },
        { (ushort)UnitCodes.NOM_DIM_BEAT_PER_MIN, "bpm" },
        { (ushort)UnitCodes.NOM_DIM_PULS_PER_MIN, "puls/min" },
        { (ushort)UnitCodes.NOM_DIM_RESP_PER_MIN, "rpm" },
        { (ushort)UnitCodes.NOM_DIM_X_M_PER_SEC, "m/sec" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_M_PER_SEC, "mm/sec" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_MIN_PER_M_SQ, "l/min/m2" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_MIN_PER_M_SQ, "ml/min/m2" },
        { (ushort)UnitCodes.NOM_DIM_SQ_X_M_PER_SEC, "m2/sec" },
        { (ushort)UnitCodes.NOM_DIM_SQ_CENTI_M_PER_SEC, "cm2/sec" },
        { (ushort)UnitCodes.NOM_DIM_CUBIC_X_M_PER_SEC, "m3/sec" },
        { (ushort)UnitCodes.NOM_DIM_CUBIC_CENTI_M_PER_SEC, "cm3/sec" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_SEC, "l/sec" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_MIN, "l/min" },
        { (ushort)UnitCodes.NOM_DIM_DECI_L_PER_MIN, "dl/min" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_MIN, "ml/min" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_HR, "l/hour" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_HR, "ml/hour" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_DAY, "l/day" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_DAY, "ml/day" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_KG, "ml/kg" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_PER_SEC, "kg/sec" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_MIN, "g/min" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_PER_MIN, "kg/min" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_MIN, "mg/min" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G_PER_MIN, "µg/min" },
        { (ushort)UnitCodes.NOM_DIM_NANO_G_PER_MIN, "ng/min" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_HR, "g/hour" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_PER_HR, "kg/hour" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_HR, "mg/hour" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G_PER_HR, "µg/hour" },
        { (ushort)UnitCodes.NOM_DIM_NANO_G_PER_HR, "ng/hr" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_DAY, "g/day" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_PER_DAY, "kg/day" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_DAY, "mg/day" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G_PER_DAY, "ug/day" },
        { (ushort)UnitCodes.NOM_DIM_NANO_G_PER_DAY, "ng/day" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_KG_PER_MIN, "g/kg/min" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_KG_PER_MIN, "mg/kg/min" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G_PER_KG_PER_MIN, "µg/kg/min" },
        { (ushort)UnitCodes.NOM_DIM_NANO_G_PER_KG_PER_MIN, "ng/kg/min" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_KG_PER_HR, "g/kg/hour" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_KG_PER_HR, "mg/kg/hour" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G_PER_KG_PER_HR, "µg/kg/hour" },
        { (ushort)UnitCodes.NOM_DIM_NANO_G_PER_KG_PER_HR, "ng/kg/hour" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_PER_L_SEC, "kg/l/sec" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_PER_M_PER_SEC, "kg/m/sec" },
        { (ushort)UnitCodes.NOM_DIM_KILO_G_M_PER_SEC, "kg-m/sec" },
        { (ushort)UnitCodes.NOM_DIM_X_NEWTON_SEC, "N-s" },
        { (ushort)UnitCodes.NOM_DIM_X_NEWTON, "N" },
        { (ushort)UnitCodes.NOM_DIM_X_PASCAL, "Pa" },
        { (ushort)UnitCodes.NOM_DIM_HECTO_PASCAL, "hPa" },
        { (ushort)UnitCodes.NOM_DIM_KILO_PASCAL, "kPa" },
        { (ushort)UnitCodes.NOM_DIM_MMHG, "mmHg" },
        { (ushort)UnitCodes.NOM_DIM_CM_H2O, "cmH2O" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_BAR, "mBar" },
        { (ushort)UnitCodes.NOM_DIM_X_JOULES, "J" },
        { (ushort)UnitCodes.NOM_DIM_EVOLT, "eV" },
        { (ushort)UnitCodes.NOM_DIM_X_WATT, "W" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_WATT, "mW" },
        { (ushort)UnitCodes.NOM_DIM_NANO_WATT, "nW" },
        { (ushort)UnitCodes.NOM_DIM_PICO_WATT, "pW" },
        { (ushort)UnitCodes.NOM_DIM_X_DYNE_PER_SEC_PER_CM5, "Dyn-sec/cm^5" },
        { (ushort)UnitCodes.NOM_DIM_X_AMPS, "A" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_AMPS, "mA" },
        { (ushort)UnitCodes.NOM_DIM_X_COULOMB, "C" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_COULOMB, "µC" },
        { (ushort)UnitCodes.NOM_DIM_X_VOLT, "V" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_VOLT, "mV" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_VOLT, "µV" },
        { (ushort)UnitCodes.NOM_DIM_X_OHM, "Ohm" },
        { (ushort)UnitCodes.NOM_DIM_OHM_K, "kOhm" },
        { (ushort)UnitCodes.NOM_DIM_X_FARAD, "F" },
        { (ushort)UnitCodes.NOM_DIM_KELVIN, "°K" },
        { (ushort)UnitCodes.NOM_DIM_FAHR, "°F" },
        { (ushort)UnitCodes.NOM_DIM_X_CANDELA, "cd" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_OSM, "mOsm" },
        { (ushort)UnitCodes.NOM_DIM_X_MOLE, "mol" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_MOLE, "mmol" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_EQUIV, "mEq" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_OSM_PER_L, "mOsm/l" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_MOLE_PER_L, "mmol/l" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_MOLE_PER_L, "µmol/l" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_EQUIV_PER_L, "mEq/l" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_EQUIV_PER_DAY, "mEq/day" },
        { (ushort)UnitCodes.NOM_DIM_X_INTL_UNIT, "i.u." },
        { (ushort)UnitCodes.NOM_DIM_MILLI_INTL_UNIT, "mi.u." },
        { (ushort)UnitCodes.NOM_DIM_X_INTL_UNIT_PER_CM_CUBE, "i.u./cm3" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_INTL_UNIT_PER_CM_CUBE, "mi.u./cm3" },
        { (ushort)UnitCodes.NOM_DIM_X_INTL_UNIT_PER_ML, "i.u./ml" },
        { (ushort)UnitCodes.NOM_DIM_X_INTL_UNIT_PER_MIN, "i.u./min" },
        { (ushort)UnitCodes.NOM_DIM_KILO_INTL_UNIT_PER_MIN, "k/min" },
        { (ushort)UnitCodes.NOM_DIM_KILO_INTL_UNIT_PER_ML, "i.u.k/ml" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_INTL_UNIT_PER_ML, "mi.u./ml" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_INTL_UNIT_PER_MIN, "mi.u./min" },
        { (ushort)UnitCodes.NOM_DIM_X_INTL_UNIT_PER_HR, "i.u./hour" },
        { (ushort)UnitCodes.NOM_DIM_KILO_INTL_UNIT_PER_HR, "i.u.k/h" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_INTL_UNIT_PER_HR, "mi.u./hour" },
        { (ushort)UnitCodes.NOM_DIM_X_INTL_UNIT_PER_KG_PER_MIN, "i.u./kg/min" },
        { (ushort)UnitCodes.NOM_DIM_KILO_INTL_UNIT_PER_KG_PER_MIN, "i.u.k/kg/min" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_INTL_UNIT_PER_KG_PER_MIN, "mi.u./kg/min" },
        { (ushort)UnitCodes.NOM_DIM_X_INTL_UNIT_PER_KG_PER_HR, "i.u./kg/hour" },
        { (ushort)UnitCodes.NOM_DIM_KILO_INTL_UNIT_PER_KG_PER_HR, "k/kg/hr" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_INTL_UNIT_PER_KG_PER_HR, "mi.u./kg/hour" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_CM_H2O, "ml/cmH2O" },
        { (ushort)UnitCodes.NOM_DIM_CM_H2O_PER_L_PER_SEC, "cmH2O/l/sec" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_SQ_PER_SEC, "ml2/sec" },
        { (ushort)UnitCodes.NOM_DIM_CM_H2O_PER_PERCENT, "cmH2O/%" },
        { (ushort)UnitCodes.NOM_DIM_DYNE_SEC_PER_M_SQ_PER_CM_5, "DS*m2/cm5" },
        { (ushort)UnitCodes.NOM_DIM_DEGC, "°C" },
        { (ushort)UnitCodes.NOM_DIM_CM_H2O_PER_L, "cmH2O/l" },
        { (ushort)UnitCodes.NOM_DIM_MM_HG_PER_PERCENT, "mmHg/%" },
        { (ushort)UnitCodes.NOM_DIM_KILO_PA_PER_PERCENT, "kPa/%" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_MM_HG, "l/mmHg" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_MM_HG, "ml/mmHg" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_AMP_HR, "mAh" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_DL, "ml/dl" },
        { (ushort)UnitCodes.NOM_DIM_DECIBEL, "dB" },
        { (ushort)UnitCodes.NOM_DIM_X_G_PER_MILLI_G, "g/mg" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_MILLI_G, "mg/mg" },
        { (ushort)UnitCodes.NOM_DIM_BEAT_PER_MIN_PER_X_L, "bpm/l" },
        { (ushort)UnitCodes.NOM_DIM_BEAT_PER_MIN_PER_MILLI_L, "bpm/ml" },
        { (ushort)UnitCodes.NOM_DIM_PER_X_L_PER_MIN, "1/(min*l)" },
        { (ushort)UnitCodes.NOM_DIM_X_M_PER_MIN, "m/min" },
        { (ushort)UnitCodes.NOM_DIM_CENTI_M_PER_MIN, "cm/min" },
        { (ushort)UnitCodes.NOM_DIM_PICO_G_PER_ML, "pg/ml" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G_PER_L, "ug/l" },
        { (ushort)UnitCodes.NOM_DIM_NANO_G_PER_L, "ng/l" },
        { (ushort)UnitCodes.NOM_DIM_PER_CUBIC_MILLI_M, "/mm3" },
        { (ushort)UnitCodes.NOM_DIM_CUBIC_MILLI_M, "mm3" },
        { (ushort)UnitCodes.NOM_DIM_X_INTL_UNIT_PER_L, "u/l" },
        { (ushort)UnitCodes.NOM_DIM_MEGA_INTL_UNIT_PER_L, "/l" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_MOL_PER_KG, "mol/kg" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_G_PER_DL, "mcg/dl" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_G_PER_L, "mg/l" },
        { (ushort)UnitCodes.NOM_DIM_PER_MICRO_L, "/ul" },
        { (ushort)UnitCodes.NOM_DIM_COMPLEX, "complx" },
        { (ushort)UnitCodes.NOM_DIM_COUNT, "count" },
        { (ushort)UnitCodes.NOM_DIM_PART, "part" },
        { (ushort)UnitCodes.NOM_DIM_PULS, "puls" },
        { (ushort)UnitCodes.NOM_DIM_UV_PP, "µV p-p" },
        { (ushort)UnitCodes.NOM_DIM_UV_SQ, "µV2" },
        { (ushort)UnitCodes.NOM_DIM_LUMEN, "lumen" },
        { (ushort)UnitCodes.NOM_DIM_LB_PER_INCH_SQ, "lb/in2" },
        { (ushort)UnitCodes.NOM_DIM_MM_HG_PER_SEC, "mmHg/s" },
        { (ushort)UnitCodes.NOM_DIM_ML_PER_SEC, "ml/s" },
        { (ushort)UnitCodes.NOM_DIM_BEAT_PER_MIN_PER_ML_C, "bpm/ml" },
        { (ushort)UnitCodes.NOM_DIM_X_JOULE_PER_DAY, "J/day" },
        { (ushort)UnitCodes.NOM_DIM_KILO_JOULE_PER_DAY, "kJ/day" },
        { (ushort)UnitCodes.NOM_DIM_MEGA_JOULE_PER_DAY, "MJ/day" },
        { (ushort)UnitCodes.NOM_DIM_X_CALORIE, "cal" },
        { (ushort)UnitCodes.NOM_DIM_KILO_CALORIE, "kcal" },
        { (ushort)UnitCodes.NOM_DIM_MEGA_CALORIE, "10**6 cal" },
        { (ushort)UnitCodes.NOM_DIM_X_CALORIE_PER_DAY, "cal/day" },
        { (ushort)UnitCodes.NOM_DIM_KILO_CALORIE_PER_DAY, "kcal/day" },
        { (ushort)UnitCodes.NOM_DIM_MEGA_CALORIE_PER_DAY, "Mcal/day" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_CALORIE_PER_DAY, "mcal/day" },
        { (ushort)UnitCodes.NOM_DIM_X_CALORIE_PER_ML, "cal/ml" },
        { (ushort)UnitCodes.NOM_DIM_KILO_CALORIE_PER_ML, "kcal/ml" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_CALORIE_PER_ML, "mcal/ml" },
        { (ushort)UnitCodes.NOM_DIM_X_JOULE_PER_ML, "J/ml" },
        { (ushort)UnitCodes.NOM_DIM_KILO_JOULE_PER_ML, "kJ/ml" },
        { (ushort)UnitCodes.NOM_DIM_X_REV_PER_MIN, "RPM" },
        { (ushort)UnitCodes.NOM_DIM_PER_L_PER_MIN_PER_KG, "l/(mn*l*kg)" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_MILLI_BAR, "l/mbar" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_MILLI_BAR, "ml/mbar" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_KG_PER_HR, "l/kg/hr" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_KG_PER_HR, "ml/kg/hr" },
        { (ushort)UnitCodes.NOM_DIM_X_BAR_PER_LITER_PER_SEC, "bar/l/s" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_BAR_PER_LITER_PER_SEC, "mbar/l/s" },
        { (ushort)UnitCodes.NOM_DIM_X_BAR_PER_LITER, "bar/l" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_BAR_PER_LITER, "mbar/l" },
        { (ushort)UnitCodes.NOM_DIM_VOLT_PER_MILLI_VOLT, "V/mV" },
        { (ushort)UnitCodes.NOM_DIM_CM_H2O_PER_MICRO_VOLT, "cmH2O/uV" },
        { (ushort)UnitCodes.NOM_DIM_X_JOULE_PER_LITER, "J/l" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_BAR, "l/bar" },
        { (ushort)UnitCodes.NOM_DIM_X_M_PER_MILLI_VOLT, "m/mV" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_M_PER_MILLI_VOLT, "mm/mV" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_MIN_PER_KG, "l/min/kg" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_MIN_PER_KG, "ml/min/kg" },
        { (ushort)UnitCodes.NOM_DIM_X_PASCAL_PER_L_PER_SEC, "Pa/l/s" },
        { (ushort)UnitCodes.NOM_DIM_HECTO_PASCAL_PER_L_PER_SEC, "hPa/l/s" },
        { (ushort)UnitCodes.NOM_DIM_KILO_PASCAL_PER_L_PER_SEC, "kPa/l/s" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_X_PASCAL, "ml/Pa" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_HECTO_PASCAL, "ml/hPa" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_KILO_PASCAL, "ml/kPa" },
        { (ushort)UnitCodes.NOM_DIM_MM_HG_PER_X_L_PER_SEC, "mmHg/l/s" },
        { (ushort)UnitCodes.NOM_DIM_X_MOLE_PER_HR, "mol/h" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_MOLE_PER_HR, "mmol/h" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_MOLE_PER_HR, "umol/h" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_BEAT, "l/beat" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_BEAT, "ml/beat" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_BEAT_PER_M_SQ, "l/beat/m2" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_BEAT_PER_M_SQ, "ml/beat/m2" },
        { (ushort)UnitCodes.NOM_DIM_X_BAR_PER_SEC, "bar/s" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_BAR_PER_SEC, "mbar/s" },
        { (ushort)UnitCodes.NOM_DIM_X_PASCAL_PER_L, "pascal/l" },
        { (ushort)UnitCodes.NOM_DIM_HECTO_PASCAL_PER_L, "hpascal/l" },
        { (ushort)UnitCodes.NOM_DIM_KILO_PASCAL_PER_L, "kpascal/l" },
        { (ushort)UnitCodes.NOM_DIM_MM_HG_PER_L, "mmHg/l" },
        { (ushort)UnitCodes.NOM_DIM_VOL_PERCENT_PER_L, "vol%/l" },
        { (ushort)UnitCodes.NOM_DIM_X_JOULE_PER_MIN, "j/min" },
        { (ushort)UnitCodes.NOM_DIM_X_MOLE_PER_ML, "mol/ml" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_MOLE_PER_ML, "mmol/ml" },
        { (ushort)UnitCodes.NOM_DIM_MICRO_MOLE_PER_ML, "umol/ml" },
        { (ushort)UnitCodes.NOM_DIM_X_BAR_PER_MIN, "bar/min" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_BAR_PER_MIN, "mbar/min" },
        { (ushort)UnitCodes.NOM_DIM_X_PASCAL_PER_MIN, "pascal/min" },
        { (ushort)UnitCodes.NOM_DIM_HECTO_PASCAL_PER_MIN, "hpascal/min" },
        { (ushort)UnitCodes.NOM_DIM_PERCENT_MIN, "%min" },
        { (ushort)UnitCodes.NOM_DIM_PERCENT_HR, "%h" },
        { (ushort)UnitCodes.NOM_DIM_X_L_PER_CM_H2O_PER_KG, "l/cmH2O/kg" },
        { (ushort)UnitCodes.NOM_DIM_MILLI_L_PER_CM_H2O_PER_KG, "ml/cmH2O/kg" },
        { (ushort)UnitCodes.NOM_DIM_CM_H2O_PER_MIN, "cm/H2O/min" },
    };
    public static string? GetValueOrDefault(
        ushort code,
        string? defaultValue)
    {
        return !CodeToUnitMap.TryGetValue(code, out var unit) ? defaultValue : unit;
    }
}