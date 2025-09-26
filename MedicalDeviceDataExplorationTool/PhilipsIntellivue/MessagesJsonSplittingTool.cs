using System.Text.RegularExpressions;

namespace MedicalDeviceDataExplorationTool.PhilipsIntellivue;

public class MessagesJsonSplittingTool
{
    private readonly Regex relativeTimeRegex = new(@"""RelativeTimeStamp"":{""Ticks"":(?<ticks>[0-9]+),""TotalSeconds"":(?<totalSeconds>[0-9.]+)}");
    private const int TicksPerSecond = 8000;

    public void Split(
        string messagesJsonFilePath)
    {
        Console.WriteLine("Splitting messages.json file...");
        var partCount = 0;
        var currentPartFilePath = GetMessagesJsonPartFilePath(messagesJsonFilePath, partCount);
        var streamWriter = new StreamWriter(currentPartFilePath);
        var hasWrittenAnyDataLines = false;
        var lastRelativeTimeTicks = 0L;
        foreach (var line in File.ReadLines(messagesJsonFilePath))
        {
            var relativeTimeMatch = relativeTimeRegex.Match(line);
            if(!relativeTimeMatch.Success)
            {
                streamWriter.WriteLine(line);
                continue;
            }

            var relativeTicks = long.Parse(relativeTimeMatch.Groups["ticks"].Value);
            if (lastRelativeTimeTicks > 0 && relativeTicks > 0
                && (relativeTicks < lastRelativeTimeTicks - GetTicks(TimeSpan.FromSeconds(60))
                    || relativeTicks > lastRelativeTimeTicks + GetTicks(TimeSpan.FromHours(2))))
            {
                streamWriter.Close();
                if(!hasWrittenAnyDataLines)
                    File.Delete(currentPartFilePath);

                partCount++;
                currentPartFilePath = GetMessagesJsonPartFilePath(messagesJsonFilePath, partCount);
                streamWriter = new StreamWriter(currentPartFilePath);
            }
            else
            {
                hasWrittenAnyDataLines = true;
            }
            streamWriter.WriteLine(line);

            lastRelativeTimeTicks = relativeTicks;
        }
        streamWriter.Close();
        if(!hasWrittenAnyDataLines)
            File.Delete(currentPartFilePath);

        Console.WriteLine("DONE");
    }

    private static long GetTicks(TimeSpan timeSpan) => ((int)timeSpan.TotalSeconds) * TicksPerSecond;

    private string GetMessagesJsonPartFilePath(
        string messagesJsonFilePath,
        int partCount)
    {
        return Path.Combine(
            Path.GetDirectoryName(messagesJsonFilePath),
            $"{Path.GetFileNameWithoutExtension(messagesJsonFilePath)}-part{partCount:00}.json");
    }
}