using System.Text.RegularExpressions;
using MedicalDeviceDataExplorationTool.PhilipsIntellivue;

if (args.Length == 0)
{
    Console.WriteLine($"Usage: {nameof(MedicalDeviceDataExplorationTool)}.exe extract <date-time of start of recording>  <messages.json file path> [numerics.csv output file path]");
    Console.WriteLine($"Usage: {nameof(MedicalDeviceDataExplorationTool)}.exe split <numerics.csv file path>");
    Console.WriteLine($"Usage: {nameof(MedicalDeviceDataExplorationTool)}.exe gedash <numerics or wave CSV file path> <output directory>");
    return;
}

var mode = args[0];
switch (mode.ToLower())
{
    case "extract":
    {
        if (args.Length < 3)
        {
            Console.WriteLine($"Usage: {nameof(MedicalDeviceDataExplorationTool)}.exe extract <date-time of start of recording>  <messages.json file path> [numerics.csv output file path]");
            return;
        }
        var startDateTime = args[1];
        var messageFilePath = args[2];
        var numericsOutputFilePath = args.Length > 3 
            ? args[3] 
            : Path.Combine(Path.GetDirectoryName(messageFilePath) ?? Environment.CurrentDirectory, "numerics-reconstructed.csv");
        new MessagesJsonDataExtractionTool().ExtractNumerics(messageFilePath, numericsOutputFilePath, startDateTime);
        return;
    }
    case "split":
    {
        if (args.Length < 2)
        {
            Console.WriteLine($"Usage: {nameof(MedicalDeviceDataExplorationTool)}.exe split <numerics.csv file path>");
            return;
        }
        var filePath = args[1];
        if (Path.GetFileNameWithoutExtension(filePath).ToLower().StartsWith("messages"))
        {
            new MessagesJsonSplittingTool().Split(filePath);
        }
        else
        {
            new NumericsSplittingTool().Split(filePath);
        }
        return;
    }
    case "gedash":
    {
        if (args.Length < 3)
        {
            Console.WriteLine($"Usage: {nameof(MedicalDeviceDataExplorationTool)}.exe gedash <numerics or wave CSV file path> <output directory>");
            return;
        }

        var numericsOrWaveFilePath = args[1];
        var outputDirectory = args[2];
        var isNumerics = Path.GetFileNameWithoutExtension(numericsOrWaveFilePath).StartsWith("numerics");
        if(isNumerics)
            new GeDashNumericsConversionTool().ConvertNumerics(numericsOrWaveFilePath, outputDirectory);
        else
            new GeDashWaveConversionTool().ConvertWaveFile(numericsOrWaveFilePath, outputDirectory);
        return;
    }
    default:
    {
        // Try to guess, what you want to do, if you dropped a file on this program.
        if (Regex.IsMatch(args[0].ToLower(), @"messages.*\.json"))
        {
            do
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1: Extract numerics");
                Console.WriteLine("2: Split");
                mode = Console.ReadLine();
                if (mode == "q")
                    return;
            } while (mode != "1" && mode != "2");

            var messagesFilePath = args[0];
            switch (mode)
            {
                case "1":
                {
                    Console.Write("Start date-time (yyyy-MM-dd HH:mm:ss): ");
                    var startDatreTime = Console.ReadLine();
                    if (startDatreTime == null)
                    {
                        Console.WriteLine("Aborted.");
                        return;
                    }

                    var numericsOutputFilePath = Path.Combine(Path.GetDirectoryName(messagesFilePath) ?? Environment.CurrentDirectory, "numerics-reconstructed.csv");
                    new MessagesJsonDataExtractionTool().ExtractNumerics(messagesFilePath, numericsOutputFilePath, startDatreTime);
                    break;
                }
                case "2":
                {
                    new MessagesJsonSplittingTool().Split(messagesFilePath);
                    break;
                }
            }
        }
        else if (Path.GetFileNameWithoutExtension(args[0]).StartsWith("numerics") && Path.GetExtension(args[0]).ToLower() == ".csv")
        {
            new NumericsSplittingTool().Split(args[0]);
        }
        else
        {
            Console.WriteLine($"Unknown mode '{mode}'. Supported: extract,split");
        }
        return;
    }
}


