using Moq;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.application.CommandHandlers;
using Roborally.core.application.CommandHandlers.UserManagement;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.User;

namespace Roborally.unitTests.Handlers;

public class SignupHandlerTests {
    [Fact]
    public async Task CannotSignupWithExistingUsername() {
        // Arrange
        var repositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var jwtServiceMock = new Mock<IJwtService>();
        var handler = new SignupCommandHandler(repositoryMock.Object, unitOfWorkMock.Object, jwtServiceMock.Object);

        // When the username already exists
        repositoryMock.Setup(r => r.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SignupCommand {
            Username = "ExistingUser",
            Password = "password123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-25))
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => 
            handler.ExecuteAsync(command, CancellationToken.None));
        
        Assert.Equal("Username already exists", exception.Message);
        Assert.Equal(409, exception.StatusCode);
        
        // Verify that AddAsync and SaveChangesAsync are never called
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CanSignupWithValidNewUsername() {
        // Arrange
        var repositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var jwtServiceMock = new Mock<IJwtService>();
        var handler = new SignupCommandHandler(repositoryMock.Object, unitOfWorkMock.Object, jwtServiceMock.Object);

        // When the username doesn't exist
        repositoryMock.Setup(r => r.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        jwtServiceMock.Setup(j => j.GenerateToken("NewUser"))
            .Returns("mock-jwt-token-67890");

        var command = new SignupCommand {
            Username = "NewUser",
            Password = "password123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-25))
        };

        // Act
        var result = await handler.ExecuteAsync(command, CancellationToken.None);

        // Assert - verify response contains username and token
        Assert.Equal("NewUser", result.Username);
        Assert.Equal("mock-jwt-token-67890", result.Token);
        
        // Verify that the user is added with HASHED password (not plain text!)
        // We check that password was hashed by verifying it's NOT equal to plain text,
        // and that it starts with BCrypt hash prefix
        repositoryMock.Verify(r => r.AddAsync(It.Is<User>(u => 
            u.Username == command.Username && 
            u.Password != command.Password &&  // Password should be hashed, NOT plain text
            u.Password.StartsWith("$2") &&  // BCrypt hashes start with $2a, $2b, or $2y
            u.Birthday == command.Birthday), 
            It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        jwtServiceMock.Verify(j => j.GenerateToken("NewUser"), Times.Once);
    }
}