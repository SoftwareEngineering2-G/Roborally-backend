using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Roborally.core.application.ApplicationContracts;

namespace Roborally.infrastructure.persistence.Authentication;

/// <summary>
/// Implementation of JWT token generation and validation
/// </summary>
public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly JwtSecurityTokenHandler _tokenHandler;

/// <author name="Gaurav Pandey 2025-11-27 21:00:00 +0100 18" />
    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    /// <summary>
    /// Generates a JWT token containing the username claim
    /// </summary>
/// <author name="Gaurav Pandey 2025-11-27 21:00:00 +0100 27" />
    public string GenerateToken(string username)
    {
        // Create claims (data stored in the token)
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
        };

        // Create signing key from secret
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Calculate expiration time
        var expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpirationInDays);

        // Create the token
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: credentials
        );

        // Return serialized token string
        return _tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Validates a JWT token and extracts the username
    /// </summary>
/// <author name="Gaurav Pandey 2025-11-27 21:00:00 +0100 61" />
    public string? ValidateToken(string token)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero // No tolerance for expired tokens
            };

            var principal = _tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal.Identity?.Name;
        }
        catch
        {
            return null; // Token is invalid
        }
    }
}