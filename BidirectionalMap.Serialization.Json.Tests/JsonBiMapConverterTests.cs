using System.Text.Json;
using System.Text.Json.Serialization;

namespace BidirectionalMap.Serialization.Json.Tests;

[TestClass]
public class JsonBiMapConverterTests
{
    [TestMethod]
    public void Serialize_NullBiMap_NullJsonValue()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonBiMapConverter<char, int>());
        
        var map     = (BiMap<char, int>?)null;
        var mapJson = JsonSerializer.Serialize(map, options);

        Assert.AreEqual("null", mapJson);
    }

    [TestMethod]
    public void Serialize_FilledBiMap_CorrectJson()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonBiMapConverter<char, int>());

        var map = new BiMap<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
            { 'c', 3 },
        };

        var expectedMapJson = @"{""a"":1,""b"":2,""c"":3}";
        var mapJson         = JsonSerializer.Serialize(map, options);

        Assert.AreEqual(expectedMapJson, mapJson);
    }

    [TestMethod]
    public void Serialize_FilledBiMapFromDictionary_BiMapJsonEqualsDictionaryJson()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonBiMapConverter<char, int>());

        var dictionary = new Dictionary<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
            { 'c', 3 },
        };

        var map            = new BiMap<char, int>(dictionary);
        var dictionaryJson = JsonSerializer.Serialize(dictionary, options);
        var mapJson        = JsonSerializer.Serialize(map, options);

        Assert.AreEqual(dictionaryJson, mapJson);
    }

    [TestMethod]
    public void Deserialize_NullJsonValue_NullBiMap()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonBiMapConverter<char, int>());

        var mapJson = "null";
        var map     = JsonSerializer.Deserialize<BiMap<char, int>>(mapJson, options);

        Assert.IsNull(map);
    }

    [TestMethod]
    public void Deserialize_FilledBiMapJson_CorrectBiMap()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonBiMapConverter<char, int>());

        var expecteMap = new BiMap<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
            { 'c', 3 },
        };

        var mapJson = @"{""a"":1,""b"":2,""c"":3}";
        var map     = JsonSerializer.Deserialize<BiMap<char, int>>(mapJson, options);

        CollectionAssert.AreEqual(expecteMap, map);
    }

    [TestMethod]
    public void Deserialize_FilledDictionaryJson_BiMapEqualsDictionary()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonBiMapConverter<char, int>());

        var dictionaryJson = @"{""a"":1,""b"":2,""c"":3}";
        var dictionary     = JsonSerializer.Deserialize<Dictionary<char, int>>(dictionaryJson, options)!;
        var map            = JsonSerializer.Deserialize<BiMap<char, int>>(dictionaryJson, options)!;

        CollectionAssert.AreEqual(dictionary, map);
    }
}