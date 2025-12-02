using Roborally.core.domain.Game.Player;

namespace Roborally.unitTests.Domain.PlayerTests;

public class PositionTests
{
    [Fact]
    public void Equals_ReturnsFalse_IfDifferentType()
    {
        var position = new Position(2, 3);
        var differentTypeObject = new object();
        
        Assert.False(position.Equals(differentTypeObject));
    }
    
    [Fact]
    public void Equals_ReturnsFalse_IfNull()
    {
        var position = new Position(2, 3);
        
        Assert.False(position.Equals(null));
    }
    
    [Fact]
    public void Equals_ReturnsFalse_IfDifferentPosition()
    {
        var position1 = new Position(2, 3);
        var position2 = new Position(5, 3);
        
        Assert.False(position1.Equals(position2));
    }
    
    [Fact]
    public void Equals_ReturnsTrue_IfSamePosition()
    {
        var position1 = new Position(2, 3);
        var position2 = new Position(2, 3);

        Assert.True(position1.Equals(position2));
    }
    
    [Fact]
    public void GetHashCode_ReturnsSameValue_ForSamePosition()
    {
        var position1 = new Position(2, 3);
        var position2 = new Position(2, 3);
        
        Assert.Equal(position1.GetHashCode(), position2.GetHashCode());
    }
}