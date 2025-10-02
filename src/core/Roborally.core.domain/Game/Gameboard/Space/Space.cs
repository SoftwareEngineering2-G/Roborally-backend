using System.Text.Json.Serialization;

namespace Roborally.core.domain.Game.Gameboard.Space;

[JsonConverter(typeof(SpaceObjectJsonConverter))]
public abstract class Space {
    public abstract string Name();
}