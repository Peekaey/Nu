using Nu_BusinessService.Helpers;

namespace Nu_Tests.Helpers;

[TestFixture]
public class Argon2PasswordHelperTests
{
    [Test]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        // Arrange
        var password = "testpassword";

        // Act
        var hashedPassword = Argon2PasswordHelper.HashPassword(password);

        // Assert
        Assert.IsNotEmpty(hashedPassword);
    }

    [Test]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        // Arrange
        var password = "testpassword";
        var hashedPassword = Argon2PasswordHelper.HashPassword(password);

        // Act
        var result = Argon2PasswordHelper.VerifyPassword(password, hashedPassword);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void VerifyPassword_ShouldReturnFalseForIncorrectPassword()
    {
        // Arrange
        var password = "testpassword";
        var hashedPassword = Argon2PasswordHelper.HashPassword(password);
        var incorrectPassword = "wrongpassword";

        // Act
        var result = Argon2PasswordHelper.VerifyPassword(incorrectPassword, hashedPassword);

        // Assert
        Assert.IsFalse(result);
    }
}
