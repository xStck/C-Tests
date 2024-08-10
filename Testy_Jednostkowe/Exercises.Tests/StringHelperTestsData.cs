using System.Collections;
using Newtonsoft.Json;

namespace Exercises.Tests;

public class StringHelperTestsData : IEnumerable<object[]>
{
    private const string  JSON_PATH = "Data/StringHelperData.json";
    public IEnumerator<object[]> GetEnumerator()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var jsonFullPath = Path.GetRelativePath(currentDir, JSON_PATH);

        if (!File.Exists(jsonFullPath))
        {
            throw new ArgumentException($"Couldn't find file: {jsonFullPath}");
        }

        var jsonData = File.ReadAllText(jsonFullPath);
        var allCases = JsonConvert.DeserializeObject<IEnumerable<object[]>>(jsonData);

        return allCases.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}