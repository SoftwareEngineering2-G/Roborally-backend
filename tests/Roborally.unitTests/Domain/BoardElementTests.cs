using Roborally.core.domain.Game.Gameboard.BoardElement;
using Roborally.core.domain.Game.Player;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class BoardElementTests
{
    [Theory]
    [MemberData(nameof(DirectionFactory.GetValidDirections), MemberType = typeof(DirectionFactory))]
    public void Gear_Clockwise_Activates_Correctly(Direction playerDirection)
    {
        // Arrange
        var player = PlayerFactory.GetValidPlayer();
        player.CurrentFacingDirection = playerDirection;
        var gear = new Gear
        {
            Direction = GearDirection.Clockwise
        };
        var initialDirection = player.CurrentFacingDirection;

        // Act
        gear.Activate(player);

        // Assert
        Assert.Equal(player.CurrentFacingDirection, initialDirection.RotateRight());
    }

    [Theory]
    [MemberData(nameof(DirectionFactory.GetValidDirections), MemberType = typeof(DirectionFactory))]
    public void Gear_CounterClockwise_Activates_Correctly(Direction playerDirection)
    {
        // Arrange
        var player = PlayerFactory.GetValidPlayer();
        player.CurrentFacingDirection = playerDirection;
        var gear = new Gear
        {
            Direction = GearDirection.CounterClockwise
        };
        var initialDirection = player.CurrentFacingDirection;

        // Act
        gear.Activate(player);

        // Assert
        Assert.Equal(player.CurrentFacingDirection, initialDirection.RotateLeft());
    }
}