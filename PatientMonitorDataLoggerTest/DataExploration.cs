using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

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
}