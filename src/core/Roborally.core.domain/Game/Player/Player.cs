using System.Runtime.InteropServices;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.GameEvents;
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

    public int RoundCount { get; set; }
    public int CurrentCheckpointPassed { get; set; } = 0;

    // Navigation property to User for accessing age/birthday
    public User.User? User { get; init; }

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
        CurrentFacingDirection = Direction.West;
        ProgrammingDeck = ProgrammingDeck.NewShuffled();
        RoundCount = 1;
    }


    internal void LockInRegisters(List<ProgrammingCard> lockedInCards, ISystemTime systemTime) {
        var lastLockedInEvent = this.GetRegistersProgrammedEvent(RoundCount);

        if (lastLockedInEvent is not null) {
            throw new Exception("You can only lock in cards once per round.");
        }

        if (lockedInCards.Count != 5) {
            throw new CustomException("You must lock in exactly 5 cards.", 400);
        }

        var lastProgrammingCardsDealtEvent = this.GetCardsDealtEvent(RoundCount);

        if (lastProgrammingCardsDealtEvent is null) {
            throw new CustomException("No cards have been dealt to the player yet.", 400);
        }

        // Ensure all lockedInCards were actually dealt
        if (lockedInCards.Any(card => !lastProgrammingCardsDealtEvent.DealtCards.Contains(card))) {
            throw new CustomException("You can only lock in cards that were dealt to you.", 400);
        }


        RegistersProgrammedEvent lockedInEvent = new RegistersProgrammedEvent() {
            HappenedAt = systemTime.CurrentTime,
            GameId = this.GameId,
            Username = this.Username,
            ProgrammedCardsInOrder = lockedInCards,
            Round = RoundCount
        };

        this.PlayerEvents.Add(lockedInEvent);
    }

    internal List<ProgrammingCard> DealProgrammingCards(int count, ISystemTime systemTime) {
        var lastDealtEvent = this.GetCardsDealtEvent(RoundCount);
        if (lastDealtEvent is not null) {
            throw new CustomException("You can only deal cards once per round", 400);
        }


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
            DealtCards = dealt,
            Round = RoundCount
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

    public Position GetNextPosition([Optional] Direction? direction) {
        return CurrentPosition.GetNext(direction ?? CurrentFacingDirection);
    }

    public Position GetPositionBehind() {
        return CurrentPosition.GetNext(CurrentFacingDirection.Opposite());
    }

    public void MoveTo(Position newPosition) {
        CurrentPosition = newPosition;
    }

    public void RecordCardExecution(ProgrammingCard card, ISystemTime systemTime) {
        CardExecutedEvent executedEvent = new CardExecutedEvent() {
            HappenedAt = systemTime.CurrentTime,
            GameId = this.GameId,
            Username = this.Username,
            Card = card,
            Round = RoundCount
        };
        this.PlayerEvents.Add(executedEvent);
    }

    public ProgrammingCard? GetLastExecutedCard() {
        return this.PlayerEvents
            .OfType<CardExecutedEvent>()
            .OrderByDescending(e => e.HappenedAt)
            .FirstOrDefault()?.Card;
    }

    public void ReachCheckpoint(Checkpoint checkpoint, ISystemTime systemTime) {
        if (checkpoint.CheckpointNumber != CurrentCheckpointPassed + 1) {
            return;
        }

        CurrentCheckpointPassed++;
        CheckpointReachedEvent reachedEvent = new CheckpointReachedEvent() {
            HappenedAt = systemTime.CurrentTime,
            GameId = this.GameId,
            Username = this.Username,
            CheckpointNumber = checkpoint.CheckpointNumber,
            Round = RoundCount
        };
        this.PlayerEvents.Add(reachedEvent);
    }

    public bool HasCompletedAllCheckpoints(int totalCheckpoints) {
        return CurrentCheckpointPassed >= totalCheckpoints;
    }
}