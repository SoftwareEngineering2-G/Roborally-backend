namespace Roborally.core.application.ApplicationContracts.GameTimer;

public interface IGameTimerService
{
    void StartProgrammingTimer(Guid gameId, TimeSpan timeSpan);
    void CancelProgrammingTimer(Guid gameId);
}