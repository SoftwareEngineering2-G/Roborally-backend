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
    /// Gets players ordered by age (oldest first = youngest birthday)
    /// </summary>
    public static List<Player.Player> GetPlayersByTurnOrder(this Game game) {
        return game.Players
            .OrderBy(p => p.User?.Birthday ?? DateOnly.MaxValue) // Earlier birthday = older = goes first, null users go last
            .ToList();
    }
}