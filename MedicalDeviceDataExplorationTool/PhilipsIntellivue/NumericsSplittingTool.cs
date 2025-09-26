namespace MedicalDeviceDataExplorationTool.PhilipsIntellivue;

public class NumericsSplittingTool
{
    public void Split(
        string numericsFilePath)
    {
        Console.WriteLine("Splitting numerics file...");
        var partCount = 0;
        var currentPartFilePath = GetNumericsPartFilePath(numericsFilePath, partCount);
        var streamWriter = new StreamWriter(currentPartFilePath);
        var hasWrittenAnyDataLines = false;
        foreach (var line in File.ReadLines(numericsFilePath))
        {
            if (line.StartsWith("Time;"))
            {
                streamWriter.Close();
                if(!hasWrittenAnyDataLines)
                    File.Delete(currentPartFilePath);

                partCount++;
                currentPartFilePath = GetNumericsPartFilePath(numericsFilePath, partCount);
                streamWriter = new StreamWriter(currentPartFilePath);
            }
            else
            {
                hasWrittenAnyDataLines = true;
            }
            streamWriter.WriteLine(line);
        }
        streamWriter.Close();
        if(!hasWrittenAnyDataLines)
            File.Delete(currentPartFilePath);
        Console.WriteLine("DONE");
    }

    private static string GetNumericsPartFilePath(
        string numericsFilePath,
        int partCount)
    {
        var currentPartFilePath = Path.Combine(
            Path.GetDirectoryName(numericsFilePath),
            $"{Path.GetFileNameWithoutExtension(numericsFilePath)}-part{partCount:00}.csv");
        return currentPartFilePath;
    }
}