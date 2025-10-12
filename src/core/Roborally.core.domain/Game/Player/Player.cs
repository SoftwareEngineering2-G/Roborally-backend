using Roborally.core.domain.Bases;
using Roborally.core.domain.Deck;
using Roborally.core.domain.Game.Player.Events;
using Roborally.core.domain.Game.Actions;

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
    
    public IAction? LastExecutedAction { get; set; }
    
    // Store the last executed card name for persistence (used by Again card)
    public string? LastExecutedCardName { get; set; }

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


    internal void LockInRegisters(List<ProgrammingCard> lockedInCards, ISystemTime systemTime) {
        
        if (lockedInCards.Count != 5) {
            throw new CustomException("You must lock in exactly 5 cards.", 400);
        }
        
        var lastProgrammingCardsDealtEvent = this.PlayerEvents
            .OfType<ProgrammingCardsDealtEvent>()
            .OrderByDescending(e => e.HappenedAt)
            .FirstOrDefault();
        
        if (lastProgrammingCardsDealtEvent is null) {
            throw new CustomException("No cards have been dealt to the player yet.", 400);
        }
        
        // Ensure all lockedInCards were actually dealt
        if (lockedInCards.Any(card => !lastProgrammingCardsDealtEvent.DealtCards.Contains(card)))
        {
            throw new CustomException("You can only lock in cards that were dealt to you.", 400);
        }
        
        RegistersProgrammedEvent lockedInEvent = new RegistersProgrammedEvent() {
            HappenedAt = systemTime.CurrentTime,
            GameId = this.GameId,
            Username = this.Username,
            ProgrammedCardsInOrder = lockedInCards
        };
        
        this.PlayerEvents.Add(lockedInEvent);
    }

    internal List<ProgrammingCard> DealProgrammingCards(int count, ISystemTime systemTime) {
        var dealt = new List<ProgrammingCard>(count);

        // Draw as many as possible from PickPiles
        dealt.AddRange(ProgrammingDeck.Draw(count));

        // If not enough, refill from discard and continue drawing
        if (dealt.Count < count && ProgrammingDeck.DiscardedPiles.Count > 0) {
            int remaining = count - dealt.Count;
            ProgrammingDeck.RefillFromDiscard();
            dealt.AddRange(ProgrammingDeck.Draw(remaining));

            // TODO: We need to add an event called DiscardPilesShuffledIntoPickPilesEvent
        }

        ProgrammingCardsDealtEvent dealtEvent = new ProgrammingCardsDealtEvent() {
            HappenedAt = systemTime.CurrentTime,
            GameId = this.GameId,
            Username = this.Username,
            DealtCards = dealt
        };
        this.PlayerEvents.Add(dealtEvent);


        return dealt;
    }

    public void RotateLeft() {
        CurrentFacingDirection = CurrentFacingDirection.RotateLeft();
    }

    public void RotateRight() {
        CurrentFacingDirection = CurrentFacingDirection.RotateRight();
    }

    public void UTurn() {
        CurrentFacingDirection = CurrentFacingDirection.Opposite();
    }

    public Position GetNextPosition(Direction direction) {
        return direction.GetNextPosition(CurrentPosition);
    }

    public Position GetPositionBehind() {
        return CurrentFacingDirection.GetPositionBehind(CurrentPosition);
    }

    public void MoveTo(Position newPosition) {
        CurrentPosition = newPosition;
    }
}