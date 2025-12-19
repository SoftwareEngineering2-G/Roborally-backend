namespace Roborally.core.application.ApplicationContracts.GameTimer;

public interface IProgrammingTimeoutHandler
{
    Task HandleTimeoutAsync(Guid gameId, CancellationToken cancellationToken = default);
}