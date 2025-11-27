namespace Roborally.core.application.ApplicationContracts;

/// <summary>
/// Service for generating and validating JWT tokens
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token for the specified username
    /// </summary>
    /// <param name="username">Username to include in the token</param>
    /// <returns>JWT token string</returns>
    string GenerateToken(string username);

    /// <summary>
    /// Validates a JWT token and extracts the username claim
    /// </summary>
    /// <param name="token">JWT token to validate</param>
    /// <returns>Username from token, or null if token is invalid</returns>
    string? ValidateToken(string token);
}
