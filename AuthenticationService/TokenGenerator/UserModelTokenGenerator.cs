using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using AuthenticationService.Services.Model;

using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.TokenGenerator;

public class UserModelTokenGenerator : ITokenGenerator<UserModel>
{
    private readonly SigningCredentials credentials;
    private readonly string issuer;

    public UserModelTokenGenerator(string key, string issuer)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        this.credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        this.issuer = issuer;
    }

    public string Generate(UserModel value, string audience, int expirationPeriodMinutes)
    {
        VerifyUserModel(value);
        VerifyAudience(audience);
        VerifyExpirationPeriod(expirationPeriodMinutes);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, value.Username!),
            new Claim(ClaimTypes.Email, value.Email!),
            new Claim(ClaimTypes.GivenName, value.GivenName!),
            new Claim(ClaimTypes.Surname, value.Surname!),
            new Claim(ClaimTypes.Role, value.Role!)
        };
        var token = new JwtSecurityToken(
            issuer: this.issuer,
            audience: audience,
            claims: claims, 
            expires: DateTime.UtcNow.AddMinutes(expirationPeriodMinutes),
            signingCredentials: this.credentials);
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }

    private void VerifyUserModel(UserModel value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), $"{nameof(UserModel)} value cannot be null");
        }
        if (value.Username == null || 
            value.Email == null || 
            value.GivenName == null || 
            value.Surname == null || 
            value.Role == null)
        {
            throw new InvalidOperationException($"Required field of {nameof(UserModel)} is null");
        }
    }

    private void VerifyAudience(string audience)
    {
        if (audience == null)
        {
            throw new ArgumentNullException(nameof(audience), "Audience must not be null");
        }
        if (audience == string.Empty)
        {
            throw new InvalidOperationException("Audience must not be empty");
        }
    }

    private void VerifyExpirationPeriod(int expirationPeriodMinutes)
    {
        if (expirationPeriodMinutes < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(expirationPeriodMinutes), "Expiration period must be positive");
        }
    }
}
