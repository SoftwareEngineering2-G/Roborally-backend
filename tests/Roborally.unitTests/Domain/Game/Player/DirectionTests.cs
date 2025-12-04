using Roborally.core.domain.Game.Player;

namespace Roborally.unitTests.Domain.PlayerTests;

public class DirectionTests
{
    [Fact]
    public void GetPositionBehind_ReturnsCorrectPosition()
    {
        var position = new Position(2, 3);

        var northDirection = Direction.North;
        var southDirection = Direction.South;
        var eastDirection = Direction.East;
        var westDirection = Direction.West;

        var northBehind = northDirection.GetPositionBehind(position);
        var southBehind = southDirection.GetPositionBehind(position);
        var eastBehind = eastDirection.GetPositionBehind(position);
        var westBehind = westDirection.GetPositionBehind(position);

        Assert.Equal(new Position(2, 4), northBehind);
        Assert.Equal(new Position(2, 2), southBehind);
        Assert.Equal(new Position(1, 3), eastBehind);
        Assert.Equal(new Position(3, 3), westBehind);
    }

    [Fact]
    public void FromDisplayName_ShouldThrow_WhenInvalidDirection()
    {
        var exception = Assert.Throws<ArgumentException>(() => Direction.FromDisplayName("InvalidDirection"));
        Assert.Contains("Invalid direction display name: InvalidDirection", exception.Message);
    }
    
    [Fact]
    public void FromDisplayName_ReturnsCorrectDirection()
    {
        var north = Direction.FromDisplayName("North");
        var south = Direction.FromDisplayName("South");
        var east = Direction.FromDisplayName("East");
        var west = Direction.FromDisplayName("West");

        Assert.Equal(Direction.North, north);
        Assert.Equal(Direction.South, south);
        Assert.Equal(Direction.East, east);
        Assert.Equal(Direction.West, west);
    }
}