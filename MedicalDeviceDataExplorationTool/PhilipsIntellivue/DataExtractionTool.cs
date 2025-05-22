using Newtonsoft.Json;
using PatientMonitorDataLogger.PhilipsIntellivue.DataProcessing;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.DataExport;
using PatientMonitorDataLogger.Shared.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace MedicalDeviceDataExplorationTool.PhilipsIntellivue;

internal class MessagesJsonDataExtractionTool
{
    public void ExtractNumerics(
        string messagesFilePath,
        string numericsOutputFilePath,
        string startTimeString)
    {
        if (!DateTime.TryParse(startTimeString, out var startDateTime))
        {
            Console.WriteLine("Invalid start date-time. Preferably use ISO date time format, i.e. yyyy-MM-dd HH:mm:ss");
            return;
        }
        Console.WriteLine("Extracting data from messages file...");
        var dataExtractor = new PhilipsIntellivueNumericsAndWavesExtractor();
        var numericsWriter = new CsvNumericsWriter(numericsOutputFilePath, measurementTypes: [
            SCADAType.NOM_ECG_CARD_BEAT_RATE,
            SCADAType.NOM_PULS_OXIM_SAT_O2,
            SCADAType.NOM_PLETH_PULS_RATE,
            SCADAType.NOM_PULS_OXIM_PERF_REL,
            SCADAType.NOM_RESP_RATE,
            SCADAType.NOM_PRESS_BLD_NONINV_SYS,
            SCADAType.NOM_PRESS_BLD_NONINV_DIA,
            SCADAType.NOM_PRESS_BLD_NONINV_MEAN,
            SCADAType.NOM_PRESS_BLD_NONINV_PULS_RATE,
            SCADAType.NOM_TEMP,
            SCADAType.NOM_PULS_RATE,
            SCADAType.NOM_PRESS_BLD_ART_ABP_MEAN,
            SCADAType.NOM_PRESS_BLD_ART_ABP_SYS,
            SCADAType.NOM_PRESS_BLD_ART_ABP_DIA,
            SCADAType.NOM_CO2_TCUT,
            SCADAType.NOM_O2_TCUT,
            SCADAType.NOM_METRIC_NOS
        ]);
        numericsWriter.Start();
        RelativeTimeTranslation RelativeTimeTranslationFactory(uint ticks) => RelativeTimeTranslation.PhilipsIntellivue(startDateTime, ticks);
        using var streamReader = new StreamReader(messagesFilePath);
        try
        {
            ReadMessagesAndWriteNumericsToCsv(streamReader, dataExtractor, numericsWriter, RelativeTimeTranslationFactory);
            Console.WriteLine("DONE");
        }
        finally
        {
            numericsWriter.Stop();
        }
    }

    private static void ReadMessagesAndWriteNumericsToCsv(
        StreamReader streamReader,
        PhilipsIntellivueNumericsAndWavesExtractor dataExtractor,
        CsvNumericsWriter numericsWriter,
        Func<uint,RelativeTimeTranslation> relativeTimeTranslation)
    {
        var messageCount = 0;
        NumericsData? linkedResultNumericsData = null;
        string? line;
        while ((line = streamReader.ReadLine()) != null)
        {
            messageCount++;
            if(messageCount % 10_000 == 0)
                Console.WriteLine($"Messages processed: {messageCount}");
            var commandMessage = JsonConvert.DeserializeObject<ICommandMessage>(line);
            if(commandMessage is not DataExportCommandMessage dataCommandMessage)
                continue;

            var isLinkedResult = dataCommandMessage.RemoteOperationHeader.Type == RemoteOperationType.LinkedResult;
            var isLastLinkedResult = dataCommandMessage.RemoteOperationData is RemoteOperationLinkedResult linkedResult
                                     && linkedResult.LinkedId.State == RemoteOperationLinkedResultState.Last;
            if (!isLastLinkedResult && linkedResultNumericsData != null)
            {
                numericsWriter.Write(linkedResultNumericsData);
                linkedResultNumericsData = null;
            }
            var dataObjects = dataExtractor.Extract(
                dataCommandMessage, 
                excludedMeasurementStates: [ MeasurementState.INVALID, MeasurementState.UNAVAILABLE ],
                relativeTimeTranslation).ToList();
            foreach (var monitorData in dataObjects)
            {
                switch (monitorData)
                {
                    case NumericsData numericsData:
                        if (isLinkedResult)
                        {
                            linkedResultNumericsData ??= new NumericsData(numericsData.Timestamp, new());
                            foreach (var (measurementType,value) in numericsData.Values)
                            {
                                linkedResultNumericsData.Values[measurementType] = value;
                            }
                        }
                        else
                        {
                            numericsWriter.Write(numericsData);
                        }
                        break;
                    case WaveData waveData:
                        //Console.WriteLine($"Message {messageCount}: Extracted wave {waveData.MeasurementType}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(monitorData));
                }
            }

            if (isLinkedResult && isLastLinkedResult && linkedResultNumericsData != null)
            {
                numericsWriter.Write(linkedResultNumericsData);
                linkedResultNumericsData = null;
            }
        }
    }
    
    
}