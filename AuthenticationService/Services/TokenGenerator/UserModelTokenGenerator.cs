using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using AuthenticationService.Services.Model;

using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Services.TokenGenerator;

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
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, value.Username!),
        };
        foreach (var role in value.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var token = new JwtSecurityToken(
            issuer: this.issuer,
            audience: audience,
            claims: claims, 
            expires: DateTime.UtcNow.AddMinutes(expirationPeriodMinutes),
            signingCredentials: this.credentials);
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}
