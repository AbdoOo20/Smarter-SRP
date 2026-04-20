using Newtonsoft.Json;

namespace Fly.Framework.Shared.Common.JsonConvertors;

public class IdentifierConverter : JsonConverter<Identifier>
{
    public override bool CanRead => true;
    public override bool CanWrite => true;

    public override Identifier ReadJson(JsonReader reader, Type objectType, Identifier existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            if (reader.Value == null) return Identifier.Empty;
            return Identifier.Parse(reader.Value!.ToString());
        }
        throw new JsonException("Invalid JSON format for Identifier.");
    }

    public override void WriteJson(JsonWriter writer, Identifier value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
}