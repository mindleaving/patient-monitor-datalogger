using System.Globalization;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace MedicalDeviceDataExplorationTool.PhilipsIntellivue;

/// <summary>
/// Converts our Philips Intellivue wave file to a format,
/// where they can be viewed in my GE Dash 4000 project (https://github.com/mindleaving/gedash).
/// The GE Dash projects contains a data explorer, which makes it fast to explore very large
/// wave and vital sign files. Can also be used to annotate data.
/// </summary>
public class GeDashWaveConversionTool
{
    private const char InputSeparator = ';';
    private const char OutputSeparator = ';';
    private static string GetOutputFileName(
        string measurementType)
        => $"GEDash_waveforms_{measurementType}.csv";

    public void ConvertWaveFile(
        string waveFilePath,
        string outputDirectory)
    {
        Console.WriteLine("Converting wave to GE Dash project format...");

        var scadaName = Path.GetFileNameWithoutExtension(waveFilePath);
        var measurementType = MapMeasurementType(scadaName);
        if (measurementType == null)
        {
            Console.WriteLine($"Could not determine measurement type for '{scadaName}' extracted from file");
            return;
        }

        var outputFileName = GetOutputFileName(measurementType);

        StreamWriter? streamWriter = null;
        DateOnly? dateOfStreamWriter = null;
        foreach (var line in File.ReadLines(waveFilePath))
        {
            var splitted = line.Split(InputSeparator);
            if(!DateTime.TryParse(splitted[0], out var timestamp))
                continue;
            var currentDate = DateOnly.FromDateTime(timestamp);
            if (dateOfStreamWriter.HasValue && dateOfStreamWriter.Value != currentDate)
            {
                streamWriter!.Close();
                streamWriter = null;
                dateOfStreamWriter = null;
            }
            if (!dateOfStreamWriter.HasValue)
            {
                dateOfStreamWriter = currentDate;
                var dateDirectory = Path.Combine(outputDirectory, dateOfStreamWriter.Value.ToString("yyyy-MM-dd"));
                var outputFilePath = Path.Combine(dateDirectory, outputFileName);
                if (!Directory.Exists(dateDirectory))
                    Directory.CreateDirectory(dateDirectory);
                streamWriter = new StreamWriter(outputFilePath);
                streamWriter.WriteLine(string.Join(OutputSeparator, [ "Timestamp", measurementType ]));
            }
            if(!double.TryParse(splitted[1], CultureInfo.InvariantCulture, out var value))
                continue;
            var secondsSince1990 = (timestamp - Year1990).TotalSeconds;
            streamWriter!.WriteLine(string.Join(OutputSeparator, [ secondsSince1990.ToString("F3", CultureInfo.InvariantCulture), value.ToString("F0") ]));
        }
        streamWriter?.Close();
        Console.WriteLine("DONE");
    }

    private string? MapMeasurementType(
        string scadaName)
    {
        return scadaName switch
        {
            nameof(SCADAType.NOM_ECG_LEAD_I) => GEDashMeasurementTypes.EcgLeadI,
            nameof(SCADAType.NOM_ECG_LEAD_II) => GEDashMeasurementTypes.EcgLeadII,
            nameof(SCADAType.NOM_ECG_ELEC_POTL) => GEDashMeasurementTypes.Ecg,
            nameof(SCADAType.NOM_ECG_ELEC_POTL_I) => GEDashMeasurementTypes.EcgLeadI,
            nameof(SCADAType.NOM_ECG_ELEC_POTL_II) => GEDashMeasurementTypes.EcgLeadII,
            nameof(SCADAType.NOM_ECG_ELEC_POTL_III) => GEDashMeasurementTypes.EcgLeadIII,
            nameof(SCADAType.NOM_PRESS_BLD_ART_ABP) => GEDashMeasurementTypes.BloodPressure,
            _ => null
        };
    }

    private static class GEDashMeasurementTypes
    {
        public const string BloodPressure = "BloodPressure";
        public const string Ecg = "Ecg";
        public const string EcgLeadI = "EcgLeadI";
        public const string EcgLeadII = "EcgLeadII";
        public const string EcgLeadIII = "EcgLeadIII";
        public const string Respiration = "Respiration";
    }

    private static readonly DateTime Year1990 = new(1990,1,1);
}