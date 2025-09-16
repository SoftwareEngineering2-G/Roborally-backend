using Moq;
using Roborally.core.application;
using Roborally.core.application.Contracts;
using Roborally.core.application.Handlers;
using Roborally.core.domain.Bases;
using Roborally.core.domain.User;

namespace Roborally.unitTests.Handlers;

public class SignupHandlerTests {
    [Fact]
    public async Task CannotSignupWithExistingUsername() {
        // Arrange
        var repositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var handler = new SignupCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);

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
        var handler = new SignupCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);

        // When the username doesn't exist
        repositoryMock.Setup(r => r.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new SignupCommand {
            Username = "NewUser",
            Password = "password123",
            Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(-25))
        };

        // Act
        var result = await handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.IsType<Guid>(result);
        Assert.NotEqual(Guid.Empty, result);
        
        // Verify that the user is added and changes are saved
        repositoryMock.Verify(r => r.AddAsync(It.Is<User>(u => 
            u.Username == command.Username && 
            u.Password == command.Password && 
            u.Birthday == command.Birthday), 
            It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}