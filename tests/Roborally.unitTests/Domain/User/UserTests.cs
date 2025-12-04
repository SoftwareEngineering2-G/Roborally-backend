using Roborally.core.application;
using Roborally.core.domain;
using Roborally.core.domain.User;
using Roborally.unitTests.Factory;

namespace Roborally.unitTests.Domain;

public class UserTests {
    [Theory]
    [MemberData(nameof(UserFactory.GetValidUsernames), MemberType = typeof(UserFactory))]
    public void UserWith_ValidUsername_CanBeCreated(string validUsername) {
        // Act
        var user = new User() {
            Username = validUsername,
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Today)
        };

        // No exception should be thrown
    }

    [Theory]
    [MemberData(nameof(UserFactory.GetInvalidUsernames), MemberType = typeof(UserFactory))]
    public void UserWith_InvalidUsername_CannotBeCreated(string invalidUsername) {
        // Act & Assert
        var exception = Assert.Throws<CustomException>(() => new User() {
            Username = invalidUsername,
            Password = "ValidPassword123",
            Birthday = DateOnly.FromDateTime(DateTime.Today)

        });
    }

    [Theory]
    [MemberData(nameof(UserFactory.GetValidPasswords), MemberType = typeof(UserFactory))]
    public void UserWith_ValidPassword_CanBeCreated(string validPassword) {
        // Act
        var user = new User() {
            Username = "ValidUsername",
            Password = validPassword,
            Birthday = DateOnly.FromDateTime(DateTime.Today)

        };

        // No exception should be thrown
    }

    [Theory]
    [MemberData(nameof(UserFactory.GetInvalidPasswords), MemberType = typeof(UserFactory))]
    public void UserWith_InvalidPassword_CannotBeCreated(string invalidPassword) {
        // Act & Assert
        Assert.Throws<CustomException>(() => new User() {
            Username = "ValidUsername",
            Password = invalidPassword,
            Birthday = DateOnly.FromDateTime(DateTime.Today)

        });
    }
}