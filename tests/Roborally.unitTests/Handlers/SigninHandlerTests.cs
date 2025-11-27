using Moq;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers;
using Roborally.core.application.CommandHandlers.UserManagement;
using Roborally.core.domain;
using Roborally.core.domain.User;

namespace Roborally.unitTests.Handlers;

public class SigninHandlerTests {
    [Fact]
    public async Task CannotSigninWithInvalidCredentials() {
        var repositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        SigninCommandHandler handler = new SigninCommandHandler(repositoryMock.Object, jwtServiceMock.Object);


        // When the user is not found
        repositoryMock.Setup(r => r.FindAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?) null);

        await Assert.ThrowsAsync<CustomException>(() => handler.ExecuteAsync(new SignInCommand() {
            Username = "DoesntExist",
            Password = "password"
        }, CancellationToken.None));
    }

    [Fact]
    public async Task CannotSigninWithWrongPassword() {
        // Arrange
        var repositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        SigninCommandHandler handler = new SigninCommandHandler(repositoryMock.Object, jwtServiceMock.Object);

        // Create a user with BCrypt hashed password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
        var existingUser = new User() {
            Username = "ValidUser",
            Password = hashedPassword,
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-25))
        };

        // When the user is found but password doesn't match
        repositoryMock.Setup(r => r.FindAsync("ValidUser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => handler.ExecuteAsync(new SignInCommand() {
            Username = "ValidUser",
            Password = "wrongpassword"
        }, CancellationToken.None));
        
        Assert.Equal("Invalid username or password", exception.Message);
        Assert.Equal(401, exception.StatusCode);
    }

    [Fact]
    public async Task CanSigninWithValidCredentials() {
        // Arrange
        var repositoryMock = new Mock<IUserRepository>();
        var jwtServiceMock = new Mock<IJwtService>();
        SigninCommandHandler handler = new SigninCommandHandler(repositoryMock.Object, jwtServiceMock.Object);

        // Create a user with BCrypt hashed password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
        var existingUser = new User() {
            Username = "ValidUser",
            Password = hashedPassword,
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-25))
        };

        // Setup mocks
        repositoryMock.Setup(r => r.FindAsync("ValidUser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);
        
        jwtServiceMock.Setup(j => j.GenerateToken("ValidUser"))
            .Returns("mock-jwt-token-12345");

        // Act
        var result = await handler.ExecuteAsync(new SignInCommand() {
            Username = "ValidUser",
            Password = "correctpassword"
        }, CancellationToken.None);

        // Assert
        Assert.Equal("ValidUser", result.Username);
        Assert.Equal("mock-jwt-token-12345", result.Token);
        repositoryMock.Verify(r => r.FindAsync("ValidUser", It.IsAny<CancellationToken>()), Times.Once);
        jwtServiceMock.Verify(j => j.GenerateToken("ValidUser"), Times.Once);
    }
}