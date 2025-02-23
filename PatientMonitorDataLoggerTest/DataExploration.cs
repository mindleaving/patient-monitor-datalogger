using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        var frame = Rs232Frame.Parse(data);
        Console.WriteLine(frame.UserData.ToString());
        var jsonSerializerSettings = new JsonSerializerSettings { Converters = { new StringEnumConverter() }};
        Console.WriteLine(JsonConvert.SerializeObject(frame.UserData, Formatting.Indented, jsonSerializerSettings));
    }
}