using FastEndpoints;

namespace Roborally.core.domain.BroadCastEvents;

public class GameCompletedBroadcastEvent : IEvent{
    public required string Winner { get; init; }
    public required Guid GameId { get; init; }
    public required Dictionary<string, int> OldRatings { get; init; }
    public required Dictionary<string, int> NewRatings { get; init; }
}