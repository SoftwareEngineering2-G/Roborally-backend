using Moq;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.QueryContracts;
using Roborally.core.application.QueryHandlers;
using Roborally.core.domain;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.QueryHandlers;

public class GetMyProfileHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetMyProfileQueryHandler _handler;

    public GetMyProfileHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetMyProfileQueryHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task CannotGetMyProfile_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((core.domain.User.User?)null);
        
        var query = new GetMyProfileQuery()
        {
            Username = "InvalidUser"
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(() => _handler.ExecuteAsync(query, CancellationToken.None));
    }
    
    [Fact]
    public async Task GetMyProfile_WhenCalled_ReturnsUserProfile()
    {
        // Arrange
        var user = UserFactory.GetValidUser();
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var query = new GetMyProfileQuery()
        {
            Username = user.Username
        };

        // Act
        await _handler.ExecuteAsync(query, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(repo => repo.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}