namespace Roborally.core.domain.Bases;

public abstract class Event {
    public int Id { get; set; }
    public required DateTime HappenedAt { get; init; }
}