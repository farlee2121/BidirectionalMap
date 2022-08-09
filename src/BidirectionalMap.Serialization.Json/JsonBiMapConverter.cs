using System.Collections.Generic;

namespace System.Text.Json.Serialization
{
    /// <summary>
    /// Converts <see cref="BiMap{TDirectKey, TReverseKey}"/> object to or from JSON.
    /// </summary>
    /// <typeparam name="TDirectKey">The type of the direct keys.</typeparam>
    /// <typeparam name="TReverseKey">The type of the reverse keys.</typeparam>
    public class JsonBiMapConverter<TDirectKey, TReverseKey> : JsonConverter<BiMap<TDirectKey, TReverseKey>?>
    {
        public override BiMap<TDirectKey, TReverseKey>? Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
        {
            var dictionary = JsonSerializer.Deserialize<Dictionary<TDirectKey, TReverseKey>>(ref reader, options);

            if (dictionary is null)
                return null;
            else
                return new BiMap<TDirectKey, TReverseKey>(dictionary);
        }

        public override void Write(Utf8JsonWriter writer,
            BiMap<TDirectKey, TReverseKey>? value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, value?.Direct, options);
    }
}
