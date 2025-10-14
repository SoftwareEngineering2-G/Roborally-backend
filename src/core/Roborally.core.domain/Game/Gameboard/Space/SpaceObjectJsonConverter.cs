using System.Text.Json;
using System.Text.Json.Serialization;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public sealed class SpaceObjectJsonConverter : JsonConverter<Space>
{
    public override Space? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected object for Space.");

        string? name = null;
        Direction[]? walls = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();

            var prop = reader.GetString();
            reader.Read();

            if (string.Equals(prop, "name", StringComparison.OrdinalIgnoreCase))
                name = reader.GetString();
            else if (string.Equals(prop, "walls", StringComparison.OrdinalIgnoreCase))
            {
                var wallStrings = JsonSerializer.Deserialize<string[]>(ref reader, options);
                if (wallStrings != null)
                {
                    walls = wallStrings.Select(Direction.FromDisplayName).ToArray();
                }
            }
            else
                reader.Skip(); // ignore unknowns
        }

        if (string.IsNullOrWhiteSpace(name))
            throw new JsonException("Space missing 'name'.");

        return SpaceFactory.FromNameAndWalls(name, walls ?? Array.Empty<Direction>());
    }

    public override void Write(Utf8JsonWriter writer, Space value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name());
        
        if (value is Space space && space.Walls().Length > 0)
        {
            writer.WritePropertyName("walls");
            JsonSerializer.Serialize(writer, space.Walls().Select(w => w.DisplayName).ToArray(), options);
        }
        
        writer.WriteEndObject();
    }
}