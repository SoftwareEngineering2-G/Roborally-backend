using Roborally.core.domain.Game.Player;

namespace Roborally.unitTests.Factory;

public class DirectionFactory
{
    public static IEnumerable<object[]> GetValidDirections()
    {
        return new List<object[]>()
        {
            new object[] { Direction.North },
            new object[] { Direction.South },
            new object[] { Direction.East },
            new object[] { Direction.West },
        };
    }
}