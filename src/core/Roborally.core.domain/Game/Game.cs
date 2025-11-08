using Roborally.core.domain.Bases;
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
    
    public bool isPaused { get; set; } = false;
    
    public int RoundCount { get; set; }
    
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

    public Dictionary<Player.Player, List<ProgrammingCard>> DealDecksToAllPlayers(ISystemTime systemTime) {
        if (!IsInProgrammingPhase()) {
            throw new CustomException("The game needs to be in programming phase", 400);
        }

        Dictionary<Player.Player, List<ProgrammingCard>> playerDealtCards = new();

        // We get what players have been dealt what cards so we can broadcast it later
        _players.ForEach(player => {
            List<ProgrammingCard> dealtCards = player.DealProgrammingCards(9, systemTime);
            playerDealtCards.Add(player, dealtCards);
        });

        return playerDealtCards;
    }
    
    public void LockInRegisters(string playerUsername, List<ProgrammingCard> lockedInCards, ISystemTime systemTime) {
        if (IsInActivationPhase()) {
            throw new CustomException("The game needs to be in programming phase", 400);
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

    public void ActivateNextBoardElement(ISystemTime systemTime) {
        if (!IsInActivationPhase()) {
            throw new CustomException("The game needs to be in activation phase", 400);
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
            BoardElementName = nextBoardElementName
        };
        GameEvents.Add(boardElementActivatedEvent);
    }

    public Player.Player ExecuteProgrammingCard(string username, ProgrammingCard card, ISystemTime systemTime) {
        if (!IsInActivationPhase()) {
            throw new CustomException("The game needs to be in activation phase", 400);
        }

        Player.Player? player = _players.Find(p => p.Username.Equals(username));
        if (player is null)
            throw new CustomException("Player does not exist", 404);

        var action = ActionFactory.CreateAction(card);
        action.Execute(player, this, systemTime);

        return player;
    }

    public Player.Player? GetNextExecutingPlayer() {
        if (!IsInActivationPhase()) {
            return null;
        }

        // Skip if no registers have been revealed yet
        if (CurrentRevealedRegister ==0) {
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

        // Get the next player (wrap around to start if at the end)
        int nextPlayerIndex = (lastPlayerIndex + 1) % playersByTurnOrder.Count;

        return playersByTurnOrder[nextPlayerIndex];
    }

    
    public void RequestPauseGame(string requestedByUsername, ISystemTime systemTime) {
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
        isPaused = allApproved;

        return new GamePauseState
        {
            result = allApproved,
            RequestedBy = pauseRequestEvent.evokedByUsername,
            PlayerResponses = responses.ToDictionary(r => r.evokedByUsername, r => r.isAnAcceptedResponse ?? false),
            RequestedAt = pauseRequestEvent.HappenedAt
        };
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
    
    private Game() {
        // EFC needs the empty constructor , i know IDE warns it but please dont delete it.
        _players = [];
    }
}