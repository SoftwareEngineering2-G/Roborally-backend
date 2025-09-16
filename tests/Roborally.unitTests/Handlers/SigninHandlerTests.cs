using Moq;
using Roborally.core.application;
using Roborally.core.application.Contracts;
using Roborally.core.application.Handlers;
using Roborally.core.domain.User;

namespace Roborally.unitTests.Handlers;

public class SigninHandlerTests {
    [Fact]
    public async Task CannotSigninWithInvalidCredentials() {
        var repositoryMock = new Mock<IUserRepository>();
        SigninCommandHandler handler = new SigninCommandHandler(repositoryMock.Object);


        // When the user is not found
        repositoryMock.Setup(r => r.FindByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?) null);

        await Assert.ThrowsAsync<CustomException>(() => handler.ExecuteAsync(new SignInCommand() {
            Username = "DoesntExist",
            Password = "password"
        }, CancellationToken.None));
    }

    [Fact]
    public async Task CanSigninWithValidCredentials() {
        // Arrange
        var repositoryMock = new Mock<IUserRepository>();
        SigninCommandHandler handler = new SigninCommandHandler(repositoryMock.Object);

        var expectedUserId = Guid.NewGuid();
        var existingUser = new User() {
            Id = expectedUserId,
            Username = "ValidUser",
            Password = "correctpassword",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-25))
        };

        // When the user is found with matching credentials
        repositoryMock.Setup(r => r.FindByUsernameAsync("ValidUser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act
        var result = await handler.ExecuteAsync(new SignInCommand() {
            Username = "ValidUser",
            Password = "correctpassword"
        }, CancellationToken.None);

        // Assert
        Assert.Equal(expectedUserId, result);
        repositoryMock.Verify(r => r.FindByUsernameAsync("ValidUser", It.IsAny<CancellationToken>()), Times.Once);
    }
}