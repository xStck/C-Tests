using System.Reflection;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace Exercises.Tests;

public class JsonFileData(string jsonPath) : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        if (testMethod == null)
            throw new ArgumentNullException(nameof(testMethod));
        
        var currentDir = Directory.GetCurrentDirectory();
        var jsonFullPath = Path.GetRelativePath(currentDir, jsonPath);

        if (!File.Exists(jsonFullPath))
        {
            throw new ArgumentException($"Couldn't find file: {jsonFullPath}");
        }

        var jsonData = File.ReadAllText(jsonFullPath);
        var allCases = JsonConvert.DeserializeObject<IEnumerable<object[]>>(jsonData);

        return allCases;
    }
}