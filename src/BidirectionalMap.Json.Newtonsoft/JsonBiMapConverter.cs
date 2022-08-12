using BidirectionalMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts <see cref="BiMap{TForwardKey, TReverseKey}"/> object to or from JSON as dictionary.
    /// <para>This converter needs to be used for BidirectionalMap v1.0.0 package only. Using this converter with new BidirectionalMap package versions is surplus.</para>
    /// </summary>
    /// <typeparam name="TForwardKey">The type of the forward keys.</typeparam>
    /// <typeparam name="TReverseKey">The type of the reverse keys.</typeparam>
    public class JsonBiMapConverter<TForwardKey, TReverseKey> : JsonConverter<BiMap<TForwardKey, TReverseKey>>
    {
        public override BiMap<TForwardKey, TReverseKey> ReadJson(JsonReader reader, Type objectType, BiMap<TForwardKey, TReverseKey> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var dictionary = serializer.Deserialize<Dictionary<TForwardKey, TReverseKey>>(reader);

            if (dictionary is null)
                return null;
            else
                return new BiMap<TForwardKey, TReverseKey>(dictionary);
        }

        public override void WriteJson(JsonWriter writer, BiMap<TForwardKey, TReverseKey> value, JsonSerializer serializer) =>
            serializer.Serialize(writer, value?.Forward?.ToDictionary(pair => pair.Key, pair => pair.Value));
    }
}
