using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest;

public class EnumSorter
{
    [Test]
    [TestCase(typeof(SCADAType))]
    public void SortUshortEnum(
        Type enumType)
    {
        if (!enumType.IsEnum)
            throw new ArgumentException("Type is not an enum");

        Sort<ushort>(enumType, x => x.ToString("X4"));
    }

    [Test]
    [TestCase(typeof(Labels))]
    public void SortUintEnum(
        Type enumType)
    {
        if (!enumType.IsEnum)
            throw new ArgumentException("Type is not an enum");
        Sort<uint>(enumType, x => x.ToString("X8"));
    }

    private void Sort<T>(
        Type enumType,
        Func<T,string> formatter) where T : IFormattable
    {
        var enumNames = Enum.GetNames(enumType);
        var enumDictionary = new Dictionary<T, string>();
        foreach (var enumName in enumNames)
        {
            var enumValue = (T)Enum.Parse(enumType, enumName);
            if (enumDictionary.TryGetValue(enumValue, out var existingEnumName))
            {
                if(existingEnumName.Length < enumName.Length)
                {
                    Console.WriteLine($"Ignoring duplicate entry {enumName}/{existingEnumName} = 0x{formatter(enumValue)}");
                    continue;
                }
                Console.WriteLine($"Replacing duplicate entry {existingEnumName} with {enumName} = 0x{formatter(enumValue)}");
            }
            enumDictionary[enumValue] = enumName;

        }
        Console.WriteLine($"public enum {enumType.Name} : {typeof(T).Name}");
        Console.WriteLine("{");
        foreach (var kvp in enumDictionary.OrderBy(x => x.Key))
        {
            Console.WriteLine($"{kvp.Value} = 0x{formatter(kvp.Key)},");
        }
        Console.WriteLine("}");
    }
}