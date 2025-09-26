using System.Reflection;
using TypescriptGenerator;
using Newtonsoft.Json.Linq;
using PatientMonitorDataLogger.API.Models;
using PatientMonitorDataLogger.Shared.Models;
using PatientMonitorDataLogger.BBraun.Models;
using PatientMonitorDataLogger.GEDash.Models;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.API.Setups;

public static class TypescriptGeneratorRunner
{
    public static void Run()
    {
        var repositoryPath = Constants.GetRepositoryPath();
        TypescriptGenerator.TypescriptGenerator.Builder
            .IncludeAllInNamespace(Assembly.GetAssembly(typeof(PatientMonitorDataSettings)), "PatientMonitorDataLogger.Shared.Models")
            .IncludeAllInNamespace(Assembly.GetAssembly(typeof(PhilipsIntellivueSettings)), "PatientMonitorDataLogger.PhilipsIntellivue.Models")
            .IncludeAllInNamespace(Assembly.GetAssembly(typeof(GEDashSettings)), "PatientMonitorDataLogger.GEDash.Models")
            .IncludeAllInNamespace(Assembly.GetAssembly(typeof(BBraunInfusionPumpSettings)), "PatientMonitorDataLogger.BBraun.Models")
            .IncludeAllInNamespace(Assembly.GetAssembly(typeof(LogSession)), "PatientMonitorDataLogger.API.Models")
            .ExcludeAllInNamespace(Assembly.GetAssembly(typeof(PatientMonitorDataSettings)), "PatientMonitorDataLogger.Shared.Models.Converters")
            .ExcludeAllInNamespace(Assembly.GetAssembly(typeof(LogSession)), "PatientMonitorDataLogger.API.Models.Converters")
            .Exclude<FrameAbortException>()
            .ReactDefaults()
            .SetDefaultFilenameForEnums("enums.ts")
            .ConfigureNamespace("PatientMonitorDataLogger.Shared.Models", options => options.Translation = "Models")
            .ConfigureNamespace("PatientMonitorDataLogger.PhilipsIntellivue.Models", options => options.Translation = "Models")
            .ConfigureNamespace("PatientMonitorDataLogger.GEDash.Models", options => options.Translation = "Models")
            .ConfigureNamespace("PatientMonitorDataLogger.BBraun.Models", options => options.Translation = "Models")
            .ConfigureNamespace("PatientMonitorDataLogger.API.Models", options => options.Translation = "Models")
            .CustomizeType(x => x == typeof(Guid), _ => "string")
            .CustomizeType(x => x == typeof(JObject), _ => "{}")
            .CustomizeType(x => x == typeof(DateOnly), _ => "Date")
            .CustomizeType(x => x == typeof(TimeOnly), _ => "string")
            .CustomizeType(x => x == typeof(char), _ => "string")
            .CustomizeType(x => x == typeof(byte), _ => "number")
            .CustomizeType(x => x == typeof(Single), _ => "number")
            //.CustomizeType(x => x == typeof(Dictionary<string,NumericsValue>), _ => "{ [measurementType: string]: Models.DataExport.NumericsValue }")
            .SetOutputDirectory(Path.Combine(repositoryPath, "patient-monitor-datalogger-frontend", "src", "types"))
            .Generate();
    }
}