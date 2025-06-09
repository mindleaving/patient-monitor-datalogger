using System.Globalization;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace MedicalDeviceDataExplorationTool.PhilipsIntellivue;

/// <summary>
/// Converts our Philips Intellivue numerics file to a format,
/// where they can be viewed in my GE Dash 4000 project (https://github.com/mindleaving/gedash).
/// The GE Dash projects contains a data explorer, which makes it fast to explore very large
/// wave and vital sign files. Can also be used to annotate data.
/// </summary>
public class GeDashNumericsConversionTool
{
    private const char InputSeparator = ';';
    private const char OutputSeparator = ';';
    private const string OutputFileName = "GEDash_vitalSigns.csv";
    private static readonly System.Collections.Generic.List<string> OutputHeader =
    [
        "Timestamp",
        GEDashMeasurementTypes.EcgHeartRate,
        GEDashMeasurementTypes.SpO2HeartRate,
        GEDashMeasurementTypes.SpO2,
        GEDashMeasurementTypes.ArterialBloodPressureHeartRate,
        GEDashMeasurementTypes.ArterialBloodPressureDiastolic,
        GEDashMeasurementTypes.ArterialBloodPressureMean,
        GEDashMeasurementTypes.ArterialBloodPressureSystolic,
        GEDashMeasurementTypes.tCO2,
        GEDashMeasurementTypes.tO2,
        GEDashMeasurementTypes.RespirationRate,
        GEDashMeasurementTypes.Temperature,

    ];

    public void ConvertNumerics(
        string numericsFilePath,
        string outputDirectory)
    {
        Console.WriteLine("Converting numerics to GE Dash project format...");

        StreamWriter? streamWriter = null;
        DateOnly? dateOfStreamWriter = null;
        Dictionary<int, string>? inputHeader = null;
        foreach (var line in File.ReadLines(numericsFilePath))
        {
            if (line.StartsWith("Time" + InputSeparator))
            {
                inputHeader = ReadTranslatedHeader(line);
                continue;
            }
            if(inputHeader == null)
                continue;
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
                var outputFilePath = Path.Combine(dateDirectory, OutputFileName);
                if (!Directory.Exists(dateDirectory))
                    Directory.CreateDirectory(dateDirectory);
                streamWriter = new StreamWriter(outputFilePath);
                streamWriter.WriteLine(string.Join(OutputSeparator, OutputHeader));
            }

            var outputValues = new string[OutputHeader.Count];
            outputValues[0] = timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            for (int inputColumnIndex = 0; inputColumnIndex < splitted.Length; inputColumnIndex++)
            {
                if(!inputHeader.TryGetValue(inputColumnIndex, out var measurementType))
                    continue;
                var outputColumnIndex = OutputHeader.IndexOf(measurementType);
                if(outputColumnIndex < 0)
                    continue;
                var valueString = splitted[inputColumnIndex];
                if(!double.TryParse(valueString, CultureInfo.InvariantCulture, out var value))
                    continue;
                if(double.IsNaN(value))
                    continue;
                outputValues[outputColumnIndex] = valueString;
            }
            streamWriter!.WriteLine(string.Join(OutputSeparator, outputValues));
        }
        streamWriter?.Close();

        Console.WriteLine("DONE");
    }

    private Dictionary<int, string>? ReadTranslatedHeader(
        string line)
    {
        var header = new Dictionary<int, string>();
        var splitted = line.Split(InputSeparator);
        for (int columnIndex = 1; columnIndex < splitted.Length; columnIndex++)
        {
            var mappedMeasurementType = MapMeasurementType(splitted[columnIndex]);
            if(mappedMeasurementType == null)
                continue;
            header.Add(columnIndex, mappedMeasurementType);
        }

        return header;
    }

    /// <summary>
    /// Converts Philips Intellivue SCADA namespace measurement names
    /// to my GE Dash 4000 project names
    /// </summary>
    private string? MapMeasurementType(
        string scadaName)
    {
        return scadaName switch
        {
            nameof(SCADAType.NOM_ECG_CARD_BEAT_RATE) => GEDashMeasurementTypes.EcgHeartRate,
            nameof(SCADAType.NOM_PULS_OXIM_SAT_O2) => GEDashMeasurementTypes.SpO2,
            nameof(SCADAType.NOM_PLETH_PULS_RATE) => GEDashMeasurementTypes.SpO2HeartRate,
            nameof(SCADAType.NOM_PRESS_BLD_NONINV_SYS) => GEDashMeasurementTypes.ArterialBloodPressureSystolic,
            nameof(SCADAType.NOM_PRESS_BLD_NONINV_DIA) => GEDashMeasurementTypes.ArterialBloodPressureDiastolic,
            nameof(SCADAType.NOM_PRESS_BLD_NONINV_MEAN) => GEDashMeasurementTypes.ArterialBloodPressureMean,
            nameof(SCADAType.NOM_PRESS_BLD_NONINV_PULS_RATE) => GEDashMeasurementTypes.ArterialBloodPressureHeartRate,
            nameof(SCADAType.NOM_PRESS_BLD_ART_ABP_SYS) => GEDashMeasurementTypes.ArterialBloodPressureSystolic,
            nameof(SCADAType.NOM_PRESS_BLD_ART_ABP_DIA) => GEDashMeasurementTypes.ArterialBloodPressureDiastolic,
            nameof(SCADAType.NOM_PRESS_BLD_ART_ABP_MEAN) => GEDashMeasurementTypes.ArterialBloodPressureMean,
            nameof(SCADAType.NOM_TEMP) => GEDashMeasurementTypes.Temperature,
            nameof(SCADAType.NOM_RESP_RATE) => GEDashMeasurementTypes.RespirationRate,
            nameof(SCADAType.NOM_CO2_TCUT) => GEDashMeasurementTypes.tCO2,
            nameof(SCADAType.NOM_O2_TCUT) => GEDashMeasurementTypes.tO2,
            _ => null
        };
    }

    private static class GEDashMeasurementTypes
    {
        public const string ArterialBloodPressureSystolic = "BloodPressure_SystolicBloodPressure";
        public const string ArterialBloodPressureDiastolic = "BloodPressure_DiastolicBloodPressure";
        public const string ArterialBloodPressureMean = "BloodPressure_MeanArterialPressure";
        public const string ArterialBloodPressureHeartRate = "BloodPressure_HeartRate";
        public const string RespirationRate = "Respiration_RespirationRate";
        public const string Temperature = "Temperature_Temperature";
        public const string SpO2 = "SpO2_SpO2";
        public const string SpO2HeartRate = "SpO2_HeartRate";
        public const string EcgHeartRate = "Ecg_HeartRate";
        public const string tCO2 = "tCO2_pCO2";
        public const string tO2 = "tCO2_pO2";
    }
}