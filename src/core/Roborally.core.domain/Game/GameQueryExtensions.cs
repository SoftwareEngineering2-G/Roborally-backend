using Roborally.core.domain.Game.Player.Events;

namespace Roborally.core.domain.Game;

public static class GameQueryExtensions {

    /// <summary>
    /// Gets all player events for the specified round, ordered by most recent first
    /// </summary>
    public static List<PlayerEvent> GetEventsForRound(this Player.Player player, int round) {
        return player.PlayerEvents
            .Where(pe => pe.Round == round)
            .OrderByDescending(pe => pe.HappenedAt)
            .ToList();
    }

    /// <summary>
    /// Gets the most recent RegistersProgrammedEvent for the specified round
    /// </summary>
    public static RegistersProgrammedEvent? GetRegistersProgrammedEvent(this Player.Player player, int round) {
        return player.GetEventsForRound(round)
            .OfType<RegistersProgrammedEvent>()
            .FirstOrDefault();
    }

    /// <summary>
    /// Gets the ProgrammingCardsDealtEvent for the specified round
    /// </summary>
    public static ProgrammingCardsDealtEvent? GetCardsDealtEvent(this Player.Player player, int round) {
        return player.GetEventsForRound(round)
            .OfType<ProgrammingCardsDealtEvent>()
            .FirstOrDefault();
    }

    /// <summary>
    /// Gets the dealt cards for the specified round as display names
    /// </summary>
    public static List<string>? GetDealtCardsDisplayNames(this Player.Player player, int round) {
        return player.GetCardsDealtEvent(round)?.DealtCards
            .Select(card => card.DisplayName)
            .ToList();
    }

    /// <summary>
    /// Gets the programmed registers for the specified round as display names
    /// </summary>
    public static List<string>? GetProgrammedRegistersDisplayNames(this Player.Player player, int round) {
        return player.GetRegistersProgrammedEvent(round)?.ProgrammedCardsInOrder
            .Select(card => card.DisplayName)
            .ToList();
    }

    /// <summary>
    /// Gets the revealed cards (up to the current revealed register) for the specified round as display names
    /// </summary>
    public static List<string> GetRevealedCardsDisplayNames(this Player.Player player, int round, int currentRevealedRegister) {
        var programmedEvent = player.GetRegistersProgrammedEvent(round);

        if (programmedEvent is null) {
            return [];
        }

        List<string> revealedCards = [];
        for (var i = 0; i < currentRevealedRegister; i++) {
            revealedCards.Add(programmedEvent.ProgrammedCardsInOrder[i].DisplayName);
        }

        return revealedCards;
    }

    /// <summary>
    /// Checks if the player has locked in their registers for the specified round
    /// </summary>
    public static bool HasLockedRegisters(this Player.Player player, int round) {
        return player.GetRegistersProgrammedEvent(round) is not null;
    }

    /// <summary>
    /// Gets the most recent CardExecutedEvent for the specified round
    /// </summary>
    public static CardExecutedEvent? GetLastCardExecutedEvent(this Player.Player player, int round, int register)
    {
        var cardExcecutedEventsThisRound = 
            player.GetEventsForRound(round)
            .OfType<CardExecutedEvent>().ToArray();
        if (register > cardExcecutedEventsThisRound.Length) return null;
        return cardExcecutedEventsThisRound.ElementAt(cardExcecutedEventsThisRound.Length - register); // Desc order
    }

    /// <summary>
    /// Gets the last player who executed a card in the specified round for the current register
    /// Returns null if no one has executed yet
    /// </summary>
    public static Player.Player? GetLastPlayerToExecuteCard(this Game game, int round, int currentRegister) {
        // Get all players who have executed a card in this round
        var playersWithExecutions = game.Players
            .Select(p => new {
                Player = p,
                LastExecution = p.GetLastCardExecutedEvent(round, currentRegister)
            })
            .Where(x => x.LastExecution != null)
            .ToList();

        if (playersWithExecutions.Count == 0) {
            return null;
        }

        // Return the player with the most recent execution
        return playersWithExecutions
            .OrderByDescending(x => x.LastExecution!.HappenedAt)
            .First().Player;
    }

    /// <summary>
    /// Gets players ordered by turn priority.
    /// If a priority antenna exists on the board, orders by proximity to antenna (distance, then angle).
    /// Otherwise, falls back to age-based ordering (oldest first = youngest birthday).
    /// </summary>
    public static List<Player.Player> GetPlayersByTurnOrder(this Game game) {
        var antennaPosition = game.GameBoard.GetPriorityAntennaPosition();
        
        if (antennaPosition != null) {
            // Use antenna proximity ordering
            return game.Players
                .Select(p => new {
                    Player = p,
                    Distance = CalculateDistance(p.CurrentPosition, antennaPosition),
                    Angle = CalculateAngle(p.CurrentPosition, antennaPosition)
                })
                .OrderBy(x => x.Distance)
                .ThenBy(x => x.Angle)
                .Select(x => x.Player)
                .ToList();
        }
        
        // Fall back to age-based ordering
        return game.Players
            .OrderBy(p => p.User?.Birthday ?? DateOnly.MaxValue)
            .ToList();
    }
    
    private static double CalculateDistance(Player.Position from, Player.Position to) {
        int dx = to.X - from.X;
        int dy = to.Y - from.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    
    private static double CalculateAngle(Player.Position from, Player.Position to) {
        // Calculate angle from antenna to player, in degrees clockwise from North
        // North = 0°, East = 90°, South = 180°, West = 270°
        int dx = from.X - to.X;
        int dy = from.Y - to.Y;
        
        // atan2 gives angle from positive X axis, we want from North (negative Y)
        // Also note: y increases downward in array coordinates
        double radians = Math.Atan2(dx, -dy);
        double degrees = radians * (180.0 / Math.PI);
        
        // Normalize to 0-360 range
        if (degrees < 0) degrees += 360;
        
        return degrees;
    }
}