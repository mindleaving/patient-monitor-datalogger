using System.Reflection;
using TypescriptGenerator;
using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.DataExport.Models;

namespace PatientMonitorDataLogger.API.Setups;

public class TypescriptGeneratorRunner
{
    public static void Run()
    {
        var repositoryPath = Constants.GetRepositoryPath();
        TypescriptGenerator.TypescriptGenerator.Builder
            .IncludeAllInNamespace(Assembly.GetAssembly(typeof(LogSession)), "PatientMonitorDataLogger.API.Models")
            .IncludeAllInNamespace(Assembly.GetAssembly(typeof(NumericsData)), "PatientMonitorDataLogger.DataExport.Models")
            .ReactDefaults()
            .SetDefaultFilenameForEnums("enums.ts")
            .ConfigureNamespace("PatientMonitorDataLogger.API.Models", options => options.Translation = "Models")
            .ConfigureNamespace("PatientMonitorDataLogger.DataExport.Models", options => options.Translation = "Models.DataExport")
            .CustomizeType(x => x == typeof(Guid), _ => "string")
            .CustomizeType(x => x == typeof(JObject), _ => "{}")
            .CustomizeType(x => x == typeof(DateOnly), _ => "Date")
            .CustomizeType(x => x == typeof(TimeOnly), _ => "string")
            .CustomizeType(x => x == typeof(char), _ => "string")
            .CustomizeType(x => x == typeof(Dictionary<MeasurementType,NumericsValue>), _ => "{ [measurementType: string]: Models.DataExport.NumericsValue }")
            .SetOutputDirectory(Path.Combine(repositoryPath, "patient-monitor-datalogger-frontend", "src", "types"))
            .Generate();
    }
}