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

/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 29" />
    private Player() {
        // For EF Core
    }

/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 33" />
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


/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 46" />
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
        
        // Add not locked in cards to discard pile
        var notLockedInCards = new List<ProgrammingCard>( lastProgrammingCardsDealtEvent.DealtCards );
        foreach (var card in lockedInCards) {
            notLockedInCards.Remove(card);
        }
        ProgrammingDeck.DiscardedPiles.AddRange(notLockedInCards);

        RegistersProgrammedEvent lockedInEvent = new RegistersProgrammedEvent() {
            HappenedAt = systemTime.CurrentTime,
            GameId = this.GameId,
            Username = this.Username,
            ProgrammedCardsInOrder = lockedInCards,
            Round = RoundCount
        };

        this.PlayerEvents.Add(lockedInEvent);
    }

/// <author name="Vincenzo Altaserse 2025-12-18 17:40:31 +0100 86" />
        internal List<ProgrammingCard> AutoCompleteRegisters(ISystemTime systemTime) {
            var lastProgrammingCardsDealtEvent = this.GetCardsDealtEvent(RoundCount);

            if (lastProgrammingCardsDealtEvent is null) {
                throw new CustomException("No cards have been dealt to the player yet.", 400);
            }

            // Assign random cards
            var assignedCards = new List<ProgrammingCard>();
            var dealtCards = new List<ProgrammingCard>( lastProgrammingCardsDealtEvent.DealtCards );

            for (var i = 0; i < 5; i++) {
                var randomIndex = new Random().Next(int.Min(5, dealtCards.Count));
                assignedCards.Add(dealtCards[randomIndex]);
                dealtCards.RemoveAt(randomIndex);
            }

            LockInRegisters(assignedCards, systemTime);

            return assignedCards;
        }
    
/// <author name="Truong Son NGO 2025-11-28 15:36:33 +0100 108" />
    internal DealCardInfo DealProgrammingCards(int count, ISystemTime systemTime) {
        var lastDealtEvent = this.GetCardsDealtEvent(RoundCount);
        if (lastDealtEvent is not null) {
            throw new CustomException("You can only deal cards once per round", 400);
        }
        
        var dealt = new List<ProgrammingCard>(count);
        bool deckReshuffled = false;

        // Draw as many as possible from PickPiles
        dealt.AddRange(ProgrammingDeck.Draw(count));

        // If not enough, refill from discard and continue drawing
        if (dealt.Count < count && ProgrammingDeck.DiscardedPiles.Count > 0) {
            int remaining = count - dealt.Count;
            ProgrammingDeck.RefillFromDiscard();
            
            deckReshuffled = true;
            DiscardPilesShuffledIntoPickPilesEvent shuffleEvent = new DiscardPilesShuffledIntoPickPilesEvent() {
                HappenedAt = systemTime.CurrentTime,
                GameId = this.GameId,
                Username = this.Username,
                Round = RoundCount,
                NewPickPiles = ProgrammingDeck.PickPiles,
            };
            this.PlayerEvents.Add(shuffleEvent);
            
            dealt.AddRange(ProgrammingDeck.Draw(remaining));
        }

        ProgrammingCardsDealtEvent dealtEvent = new ProgrammingCardsDealtEvent() {
            HappenedAt = systemTime.CurrentTime,
            GameId = this.GameId,
            Username = this.Username,
            DealtCards = dealt,
            Round = RoundCount
        };
        this.PlayerEvents.Add(dealtEvent);

        return new DealCardInfo() {
            DealtCards = dealt,
            IsDeckReshuffled = deckReshuffled
        };
    }

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 153" />
    public void RotateLeft() {
        CurrentFacingDirection = CurrentFacingDirection.RotateLeft();
    }

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 157" />
    public void RotateRight() {
        CurrentFacingDirection = CurrentFacingDirection.RotateRight();
    }

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 161" />
    public void UTurn() {
        CurrentFacingDirection = CurrentFacingDirection.Opposite();
    }

/// <author name="Nilanjana Devkota 2025-10-14 19:37:00 +0200 165" />
    public Position GetNextPosition([Optional] Direction? direction) {
        return CurrentPosition.GetNext(direction ?? CurrentFacingDirection);
    }

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 169" />
    public Position GetPositionBehind() {
        return CurrentPosition.GetNext(CurrentFacingDirection.Opposite());
    }

/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 173" />
    public void MoveTo(Position newPosition) {
        CurrentPosition = newPosition;
    }

/// <author name="Suhani Pandey 2025-10-15 21:47:56 +0200 177" />
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

/// <author name="Suhani Pandey 2025-10-15 21:47:56 +0200 188" />
    public ProgrammingCard? GetLastExecutedCard() {
        return this.PlayerEvents
            .OfType<CardExecutedEvent>()
            .OrderByDescending(e => e.HappenedAt)
            .FirstOrDefault()?.Card;
    }

/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 195" />
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

/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 211" />
    public bool HasCompletedAllCheckpoints(int totalCheckpoints) {
        return CurrentCheckpointPassed >= totalCheckpoints;
    }
}