using Moq;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;
using Roborally.core.application.QueryHandlers;

namespace Roborally.unitTests.QueryHandlers;

public class GetGamesForUserHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly GetGamesForUserQueryHandler _handler;

    public GetGamesForUserHandlerTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _handler = new GetGamesForUserQueryHandler(_gameRepositoryMock.Object);
    }

    [Fact]
    public async Task GetGamesForUser_WhenCalled_ReturnsGames()
    {
        // Arrange
        var query = new GetGamesForUserQuery()
        {
            Username = "ValidUser"
        };

        // Act
        await _handler.ExecuteAsync(query, CancellationToken.None);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.QueryGamesForUserAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}