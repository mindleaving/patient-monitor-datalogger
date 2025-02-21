﻿namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

[Flags]
public enum ExtendedPollProfileOptions : uint
{
    None = 0,
    POLL_EXT_PERIOD_NU_1SEC = 0x80000000,
    POLL_EXT_PERIOD_NU_AVG_12SEC = 0x40000000,
    POLL_EXT_PERIOD_NU_AVG_60SEC = 0x20000000,
    POLL_EXT_PERIOD_NU_AVG_300SEC = 0x10000000,
    POLL_EXT_PERIOD_RTSA = 0x08000000,
    POLL_EXT_ENUM = 0x04000000,
    POLL_EXT_NU_PRIO_LIST = 0x02000000,
    POLL_EXT_DYN_MODALITIES = 0x01000000,
}