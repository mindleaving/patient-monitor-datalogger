namespace PatientMonitorDataLogger.BBraun.Models;

public enum LastAutoProgressError
{
    NoError = 0,
    DrugOrConcentrationNotFoundInLibrary = 1,
    ParameterInvalid = 100,
    LongNameNotFound = 101,
    CatalogNameNotFound = 102,
    DrugNameDoesntMatchDrugId = 103,
    // TODO
}