using Moq;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers.GameLobby;
using Roborally.core.domain.Bases;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.CommandHandlers.GameLobby;

public class FindPublicGameLobbyHandlerTests
{
    private readonly Mock<IGameLobbyRepository> _gameLobbyRepositoryMock;
    private readonly FindPublicGameLobbyCommandHandler _handler;

    public FindPublicGameLobbyHandlerTests()
    {
        _gameLobbyRepositoryMock = new Mock<IGameLobbyRepository>();
        _handler = new FindPublicGameLobbyCommandHandler(_gameLobbyRepositoryMock.Object);
    }

    [Fact]
    public async Task ReturnEmptyList_WhenNoPublicLobbiesFound()
    {
        // Arrange
        _gameLobbyRepositoryMock.Setup(repo => repo.FindPublicLobbiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var command = new GetActiveGameLobbiesCommand();

        // Act
        var result = await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ReturnListOfPublicLobbies_WhenPublicLobbiesExist()
    {
        // Arrange
        var user1 = UserFactory.GetValidUser();
        var user2 = UserFactory.GetValidUser();
        var systemTimeMock = new Mock<ISystemTime>();
        var lobby1 = GameLobbyFactory.GetValidGameLobby(user1, systemTimeMock.Object, "Lobby1");
        var lobby2 = GameLobbyFactory.GetValidGameLobby(user2, systemTimeMock.Object, "Lobby2");

        _gameLobbyRepositoryMock.Setup(repo => repo.FindPublicLobbiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([lobby1, lobby2]);

        var command = new GetActiveGameLobbiesCommand();

        // Act
        var result = await _handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Name == "Lobby1" && r.HostUsername == user1.Username);
        Assert.Contains(result, r => r.Name == "Lobby2" && r.HostUsername == user2.Username);
    }
}