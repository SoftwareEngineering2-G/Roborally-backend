using Roborally.core.application;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Deck;
using Roborally.core.domain.Game.Gameboard;

namespace Roborally.core.domain.Game;

public class Game
{
    public Guid GameId { get; set; }

    public string HostUsername { get; set; }

    public string Name { get; set; }

    // Store the GameBoard name as foreign key
    public string GameBoardName { get; set; } = string.Empty;

    // Navigation property to access the full GameBoard object
    public GameBoard GameBoard { get; set; } = null!;

    public GamePhase CurrentPhase { get; set; }
    
    public int CurrentRevealedRegister { get; set; } = 0; // 0 means none revealed, 1-5 for each register

    // Make sure the list is ordered in a way where we can also get the next player
    private readonly List<Player.Player> _players;

    public IReadOnlyList<Player.Player> Players => _players.AsReadOnly();

    private Game()
    {
        // EFC needs the empty constructor , i know IDE warns it but please dont delete it.
        _players = new List<Player.Player>();
    }

    public Game(Guid gameId, string hostUsername, string name, List<Player.Player> players, GameBoard gameBoard)
    {
        GameId = gameId;
        _players = players;
        GameBoardName = gameBoard.Name;
        GameBoard = gameBoard;
        CurrentPhase = GamePhase.ProgrammingPhase;
        HostUsername = hostUsername;
        Name = name;
    }

    public Dictionary<Player.Player, List<ProgrammingCard>> DealDecksToAllPlayers(ISystemTime systemTime)
    {
        if (IsActive())
        {
            throw new CustomException("The game needs to be in programming phase", 400);
        }

        Dictionary<Player.Player, List<ProgrammingCard>> playerDealtCards = new();

        // We get what players have been dealt what cards so we can broadcast it later
        _players.ForEach(player =>
        {
            List<ProgrammingCard> dealtCards = player.DealProgrammingCards(9, systemTime);
            playerDealtCards.Add(player, dealtCards);
        });

        return playerDealtCards;
    }

    private bool IsActive()
    {
        return CurrentPhase.Equals(GamePhase.ActivationPhase);
    }


    public void LockInRegisters(string playerUsername, List<ProgrammingCard> lockedInCards, ISystemTime systemTime)
    {
        if (IsActive())
        {
            throw new CustomException("The game needs to be in programming phase", 400);
        }
        Player.Player? player = _players.Find(p => p.Username.Equals(playerUsername));
        if (player is null)
            throw new CustomException("Player does not exist", 404);
        player.LockInRegisters(lockedInCards, systemTime);
    }

    public Dictionary<string, ProgrammingCard> RevealNextRegister()
    {
        if (!IsActive())
        {
            throw new CustomException("The game needs to be in activation phase", 400);
        }

        if (CurrentRevealedRegister >= 5)
        {
            throw new CustomException("All registers have already been revealed", 400);
        }

        CurrentRevealedRegister++;
        int registerIndex = CurrentRevealedRegister - 1; // 0-indexed

        // Get each player's card for this register
        Dictionary<string, ProgrammingCard> revealedCards = new();
        
        foreach (var player in _players)
        {
            var lastLockedEvent = player.PlayerEvents
                .OfType<Player.Events.RegistersProgrammedEvent>()
                .OrderByDescending(e => e.HappenedAt)
                .FirstOrDefault();

            if (lastLockedEvent != null && lastLockedEvent.ProgrammedCardsInOrder.Count > registerIndex)
            {
                revealedCards[player.Username] = lastLockedEvent.ProgrammedCardsInOrder[registerIndex];
            }
        }

        return revealedCards;
    }
}