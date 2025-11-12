using Roborally.core.domain.Game.Player;
using Roborally.core.domain.Game.Gameboard.Space;

namespace Roborally.unitTests.Domain;

public class CheckpointTests
{
    [Fact]
    public void Player_ReachesCheckpoint_WhenCorrectOrder()
    {
        // Arrange
        var player = new Player("TestPlayer", Guid.NewGuid(), new Position(0, 0), Robot.Black);
        var checkpoint1 = SpaceFactory.Checkpoint(1);
        
        // Act
        bool hasWon = player.ReachCheckpoint(checkpoint1, 3);
        
        // Assert
       Assert.Equal(1, player.CurrentCheckpointPassed);
        Assert.False(hasWon); // Should not have won yet (need all 3 checkpoints)
    }
    
    [Fact]
    public void Player_CannotSkipCheckpoints()
    {
        // Arrange
        var player = new Player("TestPlayer", Guid.NewGuid(), new Position(0, 0), Robot.Black);
        var checkpoint2 = SpaceFactory.Checkpoint(2);
      // Act - Try to reach checkpoint 2 without reaching checkpoint 1 first
        bool hasWon = player.ReachCheckpoint(checkpoint2, 3);
        
        // Assert
       Assert.Equal(0, player.CurrentCheckpointPassed); // Should still be 0
        Assert.False(hasWon);
    }
    
    [Fact]
    public void Player_WinsAfterReachingAllCheckpoints()
    {
        // Arrange
        var player = new Player("TestPlayer", Guid.NewGuid(), new Position(0, 0), Robot.Black);
        var checkpoint1 = SpaceFactory.Checkpoint(1);
        var checkpoint2 = SpaceFactory.Checkpoint(2);
        var checkpoint3 = SpaceFactory.Checkpoint(3);
        // Act - Reach all checkpoints in order
        player.ReachCheckpoint(checkpoint1, 3);
        
        player.ReachCheckpoint(checkpoint2, 3);
       
        bool hasWon = player.ReachCheckpoint(checkpoint3, 3);
       
        // Assert
        Assert.Equal(3, player.CurrentCheckpointPassed);
        Assert.True(hasWon); // Should have won after all checkpoints
    }
}
