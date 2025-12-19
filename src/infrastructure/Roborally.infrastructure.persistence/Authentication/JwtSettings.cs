namespace Roborally.infrastructure.persistence.Authentication;

/// <summary>
/// Configuration class for JWT settings, mapped from appsettings.json
/// </summary>
public class JwtSettings
{
    public required string SecretKey { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public int ExpirationInDays { get; init; } = 7;
}