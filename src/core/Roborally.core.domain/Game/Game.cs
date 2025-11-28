using FastEndpoints;
using Roborally.core.domain.Bases;
using Roborally.core.domain.BroadCastEvents;
using Roborally.core.domain.Game.CardActions;
using Roborally.core.domain.Game.Deck;
using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Game.Gameboard.BoardElement;
using Roborally.core.domain.Game.Gameboard.BoardElement.BoardElementActivationStrategies;
using Roborally.core.domain.Game.GameEvents;

namespace Roborally.core.domain.Game;

public class Game {
    public Guid GameId { get; set; }

    public string HostUsername { get; set; }

    public string Name { get; set; }

    // Store the GameBoard name as foreign key
    public string GameBoardName { get; set; } = string.Empty;

    // Navigation property to access the full GameBoard object
    public GameBoard GameBoard { get; init; }

    public GamePhase CurrentPhase { get; set; }

    public int CurrentRevealedRegister { get; set; } = 0; // 0 means none revealed, 1-5 for each register

    // Make sure the list is ordered in a way where we can also get the next player
    private readonly List<Player.Player> _players;

    public IReadOnlyList<Player.Player> Players => _players.AsReadOnly();

    public List<GameEvent> GameEvents { get; set; } = [];

    public bool IsPrivate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public bool IsPaused { get; set; } = false;

    public int RoundCount { get; set; }

    public string? Winner { get; set; }

    public Game(Guid gameId, string hostUsername, string name, List<Player.Player> players, GameBoard gameBoard,
        bool isPrivate, DateTime createdAt) {
        GameId = gameId;
        _players = players;
        GameBoardName = gameBoard.Name;
        GameBoard = gameBoard;
        CurrentPhase = GamePhase.ProgrammingPhase;
        HostUsername = hostUsername;
        Name = name;
        IsPrivate = isPrivate;
        CreatedAt = createdAt;
        RoundCount = 1;
    }

    public Dictionary<Player.Player, DealCardInfo> DealDecksToAllPlayers(ISystemTime systemTime) {
        if (!IsInProgrammingPhase()) {
            throw new CustomException("The game needs to be in programming phase", 400);
        }

        Dictionary<Player.Player, DealCardInfo> playerDealtCards = new();

        // We get what players have been dealt what cards so we can broadcast it later
        _players.ForEach(player => {
            var dealCardInfo  = player.DealProgrammingCards(9, systemTime);
            playerDealtCards.Add(player, dealCardInfo);
        });

        return playerDealtCards;
    }

    public void LockInRegisters(string playerUsername, List<ProgrammingCard> lockedInCards, ISystemTime systemTime) {
        if (IsInActivationPhase()) {
            throw new CustomException("The game needs to be in programming phase", 400);
        }

        if (IsPaused) {
            throw new CustomException("The game is currently paused", 400);
        }

        Player.Player? player = _players.Find(p => p.Username.Equals(playerUsername));
        if (player is null)
            throw new CustomException("Player does not exist", 404);
        player.LockInRegisters(lockedInCards, systemTime);
    }

    // Returns username mapped to the revealed card for that register
    public Dictionary<string, ProgrammingCard> RevealNextRegister() {
        if (!IsInActivationPhase()) {
            throw new CustomException("The game needs to be in activation phase", 400);
        }

        if (IsPaused) {
            throw new CustomException("The game is currently paused", 400);
        }

        if (CurrentRevealedRegister >= 5) {
            throw new CustomException("All registers have already been revealed", 400);
        }

        CurrentRevealedRegister++;
        int registerIndex = CurrentRevealedRegister - 1; // 0-indexed

        // Get each player's card for this register
        Dictionary<string, ProgrammingCard> revealedCards = new();

        foreach (var player in _players) {
            var lastLockedEvent = player.GetRegistersProgrammedEvent(RoundCount);

            if (lastLockedEvent is not null && lastLockedEvent.ProgrammedCardsInOrder.Count > registerIndex) {
                revealedCards[player.Username] = lastLockedEvent.ProgrammedCardsInOrder[registerIndex];
            }
        }

        return revealedCards;
    }

    public async Task ActivateNextBoardElement(ISystemTime systemTime) {
        if (!IsInActivationPhase()) {
            throw new CustomException("The game needs to be in activation phase", 400);
        }

        if (IsPaused) {
            throw new CustomException("The game is currently paused", 400);
        }

        var lastActivatedElement = GameEvents.OfType<BoardElementActivatedEvent>()
            .OrderByDescending(e => e.HappenedAt)
            .FirstOrDefault();


        string nextBoardElementName =
            BoardElementFactory.GetNextForActivation(lastActivatedElement?.BoardElementName ?? null);

        // We get all players that are on the nextBoardElementName mapped to the board element they are on
        var allRelevantPlayersMappedWithRelevantBoardElement =
            GameBoard.FilterPlayersOnBoardElements(_players, nextBoardElementName);

        var activationStrategy =
            BoardElementActicationStrategyFactory.GetActivationStrategy(nextBoardElementName);

        // Activate them all
        foreach (var (player, boardElement) in allRelevantPlayersMappedWithRelevantBoardElement) {
            // TODO: current implementation activates players in arbitrary order, should it be by turn order?
            activationStrategy.Activate(this, player, boardElement);
        }

        BoardElementActivatedEvent boardElementActivatedEvent = new BoardElementActivatedEvent() {
            HappenedAt = systemTime.CurrentTime,
            GameId = this.GameId,
            BoardElementName = nextBoardElementName,
            RoundCount = this.RoundCount
        };
        GameEvents.Add(boardElementActivatedEvent);

        if (BoardElementFactory.IsThisElementLastInQueue(nextBoardElementName)) {
            await this.CheckAndRecordCheckpoint(systemTime);

            // TODO: Here, next round should be started...
            // TODO: RoundCount++;
            // TODO: Remember to add all locked in cards to the discard pile of each player: player.ProgrammingDeck.DiscardedPiles.AddRange( LockedInCards )
            // ALso then we need to notify players somehow that a new round has started
        }
    }

    public List<Player.Player> ExecuteProgrammingCard(string username, ProgrammingCard card, ISystemTime systemTime, CardExecutionContext? context = null) {
        if (!IsInActivationPhase()) {
            throw new CustomException("The game needs to be in activation phase", 400);
        }

        if (IsPaused) {
            throw new CustomException("The game is currently paused", 400);
        }

        Player.Player? player = _players.Find(p => p.Username.Equals(username));
        if (player is null)
            throw new CustomException("Player does not exist", 404);

        // Store positions BEFORE card execution to detect which robots actually moved
        var positionsBefore = _players.ToDictionary(
            p => p.Username,
            p => new {
                X = p.CurrentPosition.X, Y = p.CurrentPosition.Y, Direction = p.CurrentFacingDirection.DisplayName
            }
        );

        var action = ActionFactory.CreateAction(card);
        action.Execute(player, this, systemTime, context);

        // Find all players that actually moved (position or direction changed)
        var affectedPlayers = _players.Where(p => {
            var before = positionsBefore[p.Username];
            return before.X != p.CurrentPosition.X ||
                   before.Y != p.CurrentPosition.Y ||
                   before.Direction != p.CurrentFacingDirection.DisplayName;
        }).ToList();

        return affectedPlayers;
    }

    public Player.Player? GetNextExecutingPlayer() {
        if (!IsInActivationPhase()) {
            return null;
        }

        // Skip if no registers have been revealed yet
        if (CurrentRevealedRegister == 0) {
            return null;
        }

        // Get players ordered by age (turn order)
        var playersByTurnOrder = this.GetPlayersByTurnOrder();

        // Get the last player who executed a card in this round
        var lastPlayerToExecute = this.GetLastPlayerToExecuteCard(RoundCount, CurrentRevealedRegister);

        // If no one has executed yet, return the first player in turn order (oldest)
        if (lastPlayerToExecute == null) {
            return playersByTurnOrder.First();
        }

        // Find the index of the last player who executed
        int lastPlayerIndex = playersByTurnOrder.FindIndex(p => p.Username == lastPlayerToExecute.Username);

        // Get the next player (null all players have played)
        return (lastPlayerIndex + 1 < playersByTurnOrder.Count)
            ? playersByTurnOrder[lastPlayerIndex + 1]
            : null;
    }

    public void RequestPauseGame(string requestedByUsername, ISystemTime systemTime) {
        if (IsPaused) {
            throw new CustomException("Game is already paused", 400);
        }

        Player.Player? requestingPlayer = _players.Find(p => p.Username.Equals(requestedByUsername));
        if (requestingPlayer is null) {
            throw new CustomException("Requesting player does not exist", 404);
        }

        PauseGameEvent requestedPauseEvent = new PauseGameEvent {
            GameId = this.GameId,
            evokedByUsername = requestedByUsername,
            isRequest = true,
            isAnAcceptedResponse = null,
            HappenedAt = systemTime.CurrentTime,
        };

        GameEvents.Add(requestedPauseEvent);
    }

    public void ResponsePauseGame(string responderUsername, bool approved, ISystemTime systemTime) {
        Player.Player? respondingPlayer = _players.Find(p => p.Username.Equals(responderUsername));
        if (respondingPlayer is null) {
            throw new CustomException("Responding player does not exist", 404);
        }

        PauseGameEvent responsePauseEvent = new PauseGameEvent {
            GameId = this.GameId,
            evokedByUsername = responderUsername,
            isRequest = false,
            isAnAcceptedResponse = approved,
            HappenedAt = systemTime.CurrentTime,
        };

        GameEvents.Add(responsePauseEvent);
    }

    public GamePauseState? GetGamePauseState() {
        var pauseRequestEvent = GameEvents.OfType<PauseGameEvent>()
            .Where(e => e.isRequest)
            .OrderByDescending(e => e.HappenedAt)
            .FirstOrDefault();

        if (pauseRequestEvent is null) {
            throw new CustomException("No pause request found", 400);
        }

        var responses = GameEvents.OfType<PauseGameEvent>()
            .Where(e => !e.isRequest && e.HappenedAt > pauseRequestEvent.HappenedAt)
            .ToList();

        if (responses.Count < _players.Count - 1) return null;

        // Require all responses to be approved to pause the game
        bool allApproved = responses.All(r => r.isAnAcceptedResponse == true);
        IsPaused = allApproved;

        return new GamePauseState {
            result = allApproved,
            RequestedBy = pauseRequestEvent.evokedByUsername,
            PlayerResponses = responses.ToDictionary(r => r.evokedByUsername, r => r.isAnAcceptedResponse ?? false),
            RequestedAt = pauseRequestEvent.HappenedAt
        };
    }

    public void ContinueGame() {
        if (!IsPaused) {
            throw new CustomException("Game is not paused", 400);
        }

        IsPaused = false;
    }

    private bool IsInActivationPhase() {
        return CurrentPhase.Equals(GamePhase.ActivationPhase);
    }

    private bool IsInProgrammingPhase() {
        return CurrentPhase.Equals(GamePhase.ProgrammingPhase);
    }

    public int? GetCurrentExecutingRegister() {
        if (!IsInActivationPhase()) {
            return null;
        }

        return CurrentRevealedRegister;
    }

    public async Task HandleGameCompleted(Player.Player player, ISystemTime systemTime) {
        this.CompletedAt = systemTime.CurrentTime;
        this.Winner = player.Username;

        var initialRatings = _players
            .Where(p => p.User is not null)
            .ToDictionary(p => p.Username, p => p.User!.Rating);

        UpdatePlayerRatings();

        // Create dictionary of username to final ratings
        var finalRatings = _players
            .Where(p => p.User is not null)
            .ToDictionary(p => p.Username, p => p.User!.Rating);

        // Create the broadcast event instance
        await new GameCompletedBroadcastEvent() {
            GameId = this.GameId,
            Winner = this.Winner,
            OldRatings = initialRatings,
            NewRatings = finalRatings,
        }.PublishAsync();

    }

    private Game() {
        // EFC needs the empty constructor , i know IDE warns it but please dont delete it.
        _players = [];
    }


    private void UpdatePlayerRatings() {
        if (Winner is null) {
            return;
        }

        const int kFactor = 32; // Standard ELO K-factor for competitive games

        // Find the winner
        var winningPlayer = _players.FirstOrDefault(p => p.Username == Winner);
        if (winningPlayer?.User is null) {
            return; // Can't update ratings if User navigation property isn't loaded
        }

        // Get all losing players
        var losingPlayers = _players.Where(p => p.Username != Winner && p.User is not null).ToList();
        if (losingPlayers.Count == 0) {
            return; // No losers to calculate against
        }

        int winnerOldRating = winningPlayer.User.Rating;
        double totalRatingChangeForWinner = 0;

        // Calculate rating changes for each matchup (winner vs each loser)
        foreach (var loser in losingPlayers) {
            int loserOldRating = loser.User!.Rating;

            // Expected score for winner against this specific loser (0 to 1)
            double expectedScoreWinner = 1.0 / (1.0 + Math.Pow(10, (loserOldRating - winnerOldRating) / 400.0));

            // Expected score for loser against winner
            double expectedScoreLoser = 1.0 - expectedScoreWinner;

            // Winner gets 1 point (won), loser gets 0 points (lost)
            // Rating change = K * (actual - expected)
            double ratingChangeForWinner = kFactor * (1.0 - expectedScoreWinner);
            double ratingChangeForLoser = kFactor * (0.0 - expectedScoreLoser);

            // Accumulate winner's total gain from all opponents
            totalRatingChangeForWinner += ratingChangeForWinner;

            // Apply loser's rating change immediately
            loser.User.Rating = Math.Max(0, loserOldRating + (int)Math.Round(ratingChangeForLoser));
        }

        // Apply winner's total rating gain
        winningPlayer.User.Rating = winnerOldRating + (int)Math.Round(totalRatingChangeForWinner);
    }
}
