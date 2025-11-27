using Moq;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;
using Roborally.core.application.QueryHandlers;

namespace Roborally.unitTests.QueryHandlers;

public class GetLeaderboardHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetLeaderboardQueryHandler _handler;

    public GetLeaderboardHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetLeaderboardQueryHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetLeaderboard_WhenCalled_ReturnsLeaderboard()
    {
        // Arrange
        var query = new GetLeaderboardQuery();

        var expectedResult = new GetLeaderboardQueryResult
        {
            Items =
            [
                new GetLeaderboardQueryResponse() { Username = "User1", Rating = 100 },
                new GetLeaderboardQueryResponse() { Username = "User2", Rating = 90 }
            ],
            TotalCount = 2,
            PageSize = 10,
            CurrentPage = 1,
            TotalPages = 1
        };

        _userRepositoryMock
            .Setup(repo => repo.GetLeaderboardQueryAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _handler.ExecuteAsync(query, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult, result);
        _userRepositoryMock.Verify(repo => repo.GetLeaderboardQueryAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}