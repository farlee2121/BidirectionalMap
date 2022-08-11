using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;

namespace BidirectionalMap.Serialization.Json.Tests;

[TestClass]
public class JsonBiMapConverterTests
{
    [TestMethod]
    public void Serialize_NullMap_NullJsonValue()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new JsonBiMapConverter<char, int>());
        
        var map     = (BiMap<char, int>?)null;
        var mapJson = JsonConvert.SerializeObject(map, settings);

        Assert.AreEqual("null", mapJson);
    }

    [TestMethod]
    public void Serialize_FilledMap_CorrectJson()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new JsonBiMapConverter<char, int>());

        var map = new BiMap<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
            { 'c', 3 },
        };

        var expectedMapJson = @"{""a"":1,""b"":2,""c"":3}";
        var mapJson         = JsonConvert.SerializeObject(map, settings);

        Assert.AreEqual(expectedMapJson, mapJson);
    }

    [TestMethod]
    public void Serialize_FilledMapFromDictionary_BiMapJsonEqualsDictionaryJson()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new JsonBiMapConverter<char, int>());

        var dictionary = new Dictionary<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
            { 'c', 3 },
        };

        var map            = new BiMap<char, int>(dictionary);
        var dictionaryJson = JsonConvert.SerializeObject(dictionary, settings);
        var mapJson        = JsonConvert.SerializeObject(map, settings);

        Assert.AreEqual(dictionaryJson, mapJson);
    }

    [TestMethod]
    public void Deserialize_NullJsonValue_NullMap()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new JsonBiMapConverter<char, int>());

        var mapJson = "null";
        var map     = JsonConvert.DeserializeObject<BiMap<char, int>>(mapJson, settings);

        Assert.IsNull(map);
    }

    [TestMethod]
    public void Deserialize_FilledBiMapJson_CorrectMap()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new JsonBiMapConverter<char, int>());

        var expecteMap = new BiMap<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
            { 'c', 3 },
        };

        var mapJson = @"{""a"":1,""b"":2,""c"":3}";
        var map     = JsonConvert.DeserializeObject<BiMap<char, int>>(mapJson, settings)!;

        CollectionAssert.AreEqual((ICollection)expecteMap.Forward.Keys, (ICollection)map.Forward.Keys);
        CollectionAssert.AreEqual((ICollection)expecteMap.Forward.Values, (ICollection)map.Forward.Values);
        CollectionAssert.AreEqual((ICollection)expecteMap.Reverse.Keys, (ICollection)map.Reverse.Keys);
        CollectionAssert.AreEqual((ICollection)expecteMap.Reverse.Values, (ICollection)map.Reverse.Values);
    }

    [TestMethod]
    public void Deserialize_FilledDictionaryJson_MapEqualsDictionary()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new JsonBiMapConverter<char, int>());

        var dictionaryJson = @"{""a"":1,""b"":2,""c"":3}";
        var dictionary     = JsonConvert.DeserializeObject<Dictionary<char, int>>(dictionaryJson, settings)!;
        var map            = JsonConvert.DeserializeObject<BiMap<char, int>>(dictionaryJson, settings)!;

        CollectionAssert.AreEqual(dictionary.Keys, (ICollection)map.Forward.Keys);
        CollectionAssert.AreEqual(dictionary.Values, (ICollection)map.Forward.Values);
    }
}