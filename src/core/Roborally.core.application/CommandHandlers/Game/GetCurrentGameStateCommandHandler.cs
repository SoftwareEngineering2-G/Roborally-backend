using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Gameboard.BoardElement;

namespace Roborally.core.application.CommandHandlers.Game;

public class
    GetCurrentGameStateCommandHandler : ICommandHandler<GetCurrentGameStateCommand,
    GetCurrentGameStateCommandResponse> {
    private readonly IGameRepository _gameRepository;


    public GetCurrentGameStateCommandHandler(IGameRepository gameRepository) {
        _gameRepository = gameRepository;
    }


    public async Task<GetCurrentGameStateCommandResponse> ExecuteAsync(GetCurrentGameStateCommand command,
        CancellationToken ct) {
        domain.Game.Game? game = await _gameRepository.FindAsync(command.GameId, ct);

        if (game is null) {
            throw new CustomException("Game does not exist", 404);
        }

        GetCurrentGameStateCommandResponse.MyState personalState;
        if (game.CurrentPhase.Equals(GamePhase.ActivationPhase)) {
            personalState =
                new GetCurrentGameStateCommandResponse.MyState(null, null);
        }
        else {
            var player = game.Players.FirstOrDefault(p => p.Username.Equals(command.Username));

            var cardsDealt = player?.GetDealtCardsDisplayNames(game.RoundCount);
            var programmedRegisters = player?.GetProgrammedRegistersDisplayNames(game.RoundCount);

            personalState = new GetCurrentGameStateCommandResponse.MyState(programmedRegisters, cardsDealt);
        }

        return new GetCurrentGameStateCommandResponse() {
            GameId = game.GameId.ToString(),
            HostUsername = game.HostUsername,
            Name = game.Name,
            CurrentPhase = game.CurrentPhase.DisplayName,
            IsPrivate = game.IsPrivate,
            CurrentRevealedRegister = game.CurrentRevealedRegister,
            CurrentTurnUsername = game.GetNextExecutingPlayer()?.Username,
            CurrentExecutingRegister = game.GetCurrentExecutingRegister(),
            GameBoard = new GetCurrentGameStateCommandResponse.GameBoardSpaces(game.GameBoard.Name,
                game.GameBoard.Spaces.Select(row =>
                        row.Select(space => {
                            // Get direction for board elements
                            string? direction = space switch {
                                BlueConveyorBelt blueBelt => blueBelt.Direction.DisplayName,
                                GreenConveyorBelt greenBelt => greenBelt.Direction.DisplayName,
                                Gear gear => gear.Direction.DisplayName,
                                _ => null
                            };

                            return new GetCurrentGameStateCommandResponse.Space(
                                space.Name(),
                                space.Walls().Select(wall => wall.DisplayName).ToList(),
                                direction);
                        }).ToArray())
                    .ToArray()),
            Players = game.Players
                .Select(p => {
                    var hasLockedRegisters = p.HasLockedRegisters(game.RoundCount);
                    var revealedCardsInOrder = p.GetRevealedCardsDisplayNames(game.RoundCount, game.CurrentRevealedRegister);

                    return new GetCurrentGameStateCommandResponse.Player(
                        p.Username,
                        p.Robot.DisplayName,
                        p.CurrentPosition.X,
                        p.CurrentPosition.Y,
                        p.CurrentFacingDirection.DisplayName,
                        hasLockedRegisters,
                        revealedCardsInOrder,
                        p.CurrentCheckpointPassed
                    );
                }).ToList(),
            PersonalState = personalState
        };
    }
}