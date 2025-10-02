using System.Text.Json;
using System.Text.Json.Serialization;

namespace Roborally.core.domain.Game.Gameboard.Space;

public sealed class SpaceObjectJsonConverter : JsonConverter<Space>
{
    public override Space? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected object for Space.");

        string? name = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();

            var prop = reader.GetString();
            reader.Read();

            if (string.Equals(prop, "name", StringComparison.OrdinalIgnoreCase))
                name = reader.GetString();
            else
                reader.Skip(); // ignore unknowns
        }

        if (string.IsNullOrWhiteSpace(name))
            throw new JsonException("Space missing 'name'.");

        return SpaceFactory.FromName(name);
    }

    public override void Write(Utf8JsonWriter writer, Space value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name());
        writer.WriteEndObject();
    }
}