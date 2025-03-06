using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest;

/// <summary>
/// Methods for exploring Philips Intellivue communication data
/// </summary>
public class DataExploration
{
    [Test]
    [TestCase("BakGFwaaBywHxAhZCOUJYQnKCh4KXgqMCqkKtwq2CqgKjwpsCj8KCgnNCYkJPgjuCJwISgf7B7IHbwc2BwcG5A==")]
    public void DeconstructObservedValueArray(
        string dataString)
    {
        var data = Convert.FromBase64String(dataString);
        foreach (var bPair in data.Chunk(2))
        {
            var value = BigEndianBitConverter.ToUInt16(bPair);
            Console.WriteLine($"{value}");
        }
    }

    [Test]
    public void DebugFaultyFrameWithPatientDemographicsResult()
    {
        var data = Convert.FromBase64String("wBEBASThAAACAAIBHAAAAAcBFgAhAAAAAAwWAQwAAAWe8gD//////////wABACoIBwABAPQAAAABAO4AUAAaAOgJIQACAFAJVwACAAIKGgACIADwAQAKyNnSEQ40UpooXAliAAIAAwoeAAIAAAldAAQAAgAACV8ABAACAAAJXAAQAA4ASwBvAGUAbgBpAGcAAPLUAAQAAgAACVoACgAIADEAMgAzAADy4QAEAAIAAPEpAAQAAgAA8SoABAACAAAJYQACAAIJWAAIAAAAAAAAAAAJ2AAGAH///wlACdwABgB///8FEQnfAAYAf///BsAJVgAGAH///wXA8ewAAgAB+egACAAAAAAAAAAA+ekAAvEL84oABAAAAADy4gAEgHUglfLjAASAiw2ZgmXB");
        var frame = PhilipsIntellivueFrame.Parse(data);
        Console.WriteLine(frame.UserData.ToString());
        var jsonSerializerSettings = new JsonSerializerSettings { Converters = { new StringEnumConverter() }};
        Console.WriteLine(JsonConvert.SerializeObject(frame.UserData, Formatting.Indented, jsonSerializerSettings));
    }

    [Test]
    public void ConvertSampleDataToBase64()
    {
        var data = new ushort[]
        {
        };
        var bytes = data.SelectMany(BitConverter.GetBytes).ToArray();
        var dataString = Convert.ToBase64String(bytes);
        Console.WriteLine(dataString);
    }

    [Test]
    [TestCase(@"F:\data\BraunSpaceStation\data\multiple-events\bbraun-bcc-messages.txt")]
    [TestCase(@"F:\data\BraunSpaceStation\data\stuffing-off-request-mode\bbraun-bcc-messages_1.txt")]
    public void BBraunBccResponseToCsv(string inputFilePath)
    {
        var stringMessages = File.ReadAllLines(inputFilePath);
        var output = new System.Collections.Generic.List<string> { "Time,Pump,Parameter,Value" };
        foreach (var stringMessage in stringMessages)
        {
            var rawMessage = StringMessageHelpers.HumanFriendlyControlCharactersToRaw(stringMessage);
            var informationStartIndex = rawMessage.IndexOf("/1/1>", StringComparison.InvariantCulture);
            var information = rawMessage[(informationStartIndex + 5)..^7];
            var quadruples = information.Split(BBraunBccResponse.RecordSeparator);
            output.AddRange(quadruples);
        }

        var outputFilePath = inputFilePath.Replace(".txt", ".csv");
        if (outputFilePath == inputFilePath)
            outputFilePath += ".csv";
        File.WriteAllLines(outputFilePath, output);
    }
}