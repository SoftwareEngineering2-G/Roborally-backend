using Roborally.core.application;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Deck;
using Roborally.core.domain.Game.Player.Events;

namespace Roborally.core.domain.Game.Player;

public class Player {
    public string Username { get; init; }
    public Guid GameId { get; init; }
    public Robot Robot { get; init; }

    public Direction CurrentFacingDirection { get; set; }
    public Position CurrentPosition { get; set; }

    public Position SpawnPosition { get; init; }

    public ProgrammingDeck ProgrammingDeck { get; init; }
    public List<PlayerEvent> PlayerEvents { get; init; } = [];

    private Player() {
        // For EF Core
    }

    public Player(string username, Guid gameId, Position spawnPosition, Robot robot) {
        Username = username;
        GameId = gameId;
        SpawnPosition = spawnPosition;
        // When a player is created, they start at their spawn position
        CurrentPosition = spawnPosition;
        Robot = robot;
        CurrentFacingDirection = Direction.North;
        ProgrammingDeck = ProgrammingDeck.NewShuffled();
    }


    public void LockInRegisters(List<ProgrammingCard> lockedInCards, ISystemTime systemTime) {
        if (lockedInCards.Count != 5) {
            throw new CustomException("You must lock in exactly 5 cards.", 400);
        }

        RegistersProgrammedEvent lockedInEvent = new RegistersProgrammedEvent() {
            HappenedAt = systemTime.CurrentTime,
            GameId = this.GameId,
            Username = this.Username,
            ProgrammedCardsInOrder = lockedInCards
        };
        
        this.PlayerEvents.Add(lockedInEvent);
    }
}