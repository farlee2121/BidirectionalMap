using BidirectionalMap;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts <see cref="BiMap{TDirectKey, TReverseKey}"/> object to or from JSON.
    /// <para>This converter may be used for BidirectionalMap v1.0.0 package only. In upper package versions <see cref="BiMap{TForwardKey, TReverseKey}"/> implements <see cref="IDictionary{TKey, TValue}"/>.</para>
    /// </summary>
    /// <typeparam name="TDirectKey">The type of the direct keys.</typeparam>
    /// <typeparam name="TReverseKey">The type of the reverse keys.</typeparam>
    public class JsonBiMapConverter<TDirectKey, TReverseKey> : JsonConverter<BiMap<TDirectKey, TReverseKey>>
    {
        public override BiMap<TDirectKey, TReverseKey> ReadJson(JsonReader reader, Type objectType, BiMap<TDirectKey, TReverseKey> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var dictionary = serializer.Deserialize<Dictionary<TDirectKey, TReverseKey>>(reader);

            if (dictionary is null)
                return null;
            else
                return new BiMap<TDirectKey, TReverseKey>(dictionary);
        }

        public override void WriteJson(JsonWriter writer, BiMap<TDirectKey, TReverseKey> value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.Forward?.ToDictionary());
        }
    }
}
