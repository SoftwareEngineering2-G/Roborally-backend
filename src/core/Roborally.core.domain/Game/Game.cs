using Roborally.core.application;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Deck;
using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.Player;

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

    public void AssignSpawnsInOrder(IReadOnlyList<Player> players)
    {
        // Find all spawn tiles + coords
        var spawns = new List<(int x, int y, SpawnPoint sp)>();
        for (int y = 0; y < GameBoard.Space.Length; y++)
        {
            for (int x = 0; x < GameBoard.Space[y].Length; x++)
            {
                if (GameBoard.Space[y][x] is SpawnPoint sp) spawns.Add((x, y, sp));
            }
        }

        // Deterministic order
        spawns.Sort((a, b) => a.sp.Index.CompareTo(b.sp.Index));

        if (spawns.Count == 0) throw new CustomException("No spawn points on board.", 500);

        // Assign in order; wrap if more players than spawns
        for (int i = 0; i < players.Count; i++)
        {
            var (x, y, _) = spawns[i % spawns.Count];
            var p = players[i];

            p.SpawnPosition = new Player.Position(x, y);
            p.MoveTo(p.SpawnPosition, this);
        }
    }

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

        AssignSpawnsInOrder(_players); // spawnpoints
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

