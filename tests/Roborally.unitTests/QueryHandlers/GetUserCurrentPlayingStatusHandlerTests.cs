using Moq;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;
using Roborally.core.application.QueryHandlers;

namespace Roborally.unitTests.QueryHandlers;

public class GetUserCurrentPlayingStatusHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly GetUserCurrentPlayingStatusQueryHandler _handler;

    public GetUserCurrentPlayingStatusHandlerTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _handler = new GetUserCurrentPlayingStatusQueryHandler(_gameRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUserCurrentPlayingStatus_WhenCalled_ReturnsCurrentPlayingStatus()
    {
        // Arrange
        var response = new GetCurrentUserPlayingStatusResponse()
        {
            GameId = Guid.NewGuid(),
            IsCurrentlyOnAGame = true,
            IsCurrentlyOnAGameLobby = true
        };

        _gameRepositoryMock.Setup(repo =>
                repo.QueryUserCurrentPlayingStatusAsync(It.IsAny<GetUserCurrentPlayingStatusQuery>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var query = new GetUserCurrentPlayingStatusQuery()
        {
            Username = "ValidUser"
        };

        // Act
        await _handler.ExecuteAsync(query, CancellationToken.None);

        // Assert
        _gameRepositoryMock.Verify(
            repo => repo.QueryUserCurrentPlayingStatusAsync(query, It.IsAny<CancellationToken>()), Times.Once);
    }
}