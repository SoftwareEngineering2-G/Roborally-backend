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
}